using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace PedestrianDetectionTrainer
{
    public partial class Form1 : Form
    {
        private string initialDirectory;
        private List<string> imagePaths;
        private string trainingFile;
        private bool loadImgEnabled = true;
        private bool loadTrainFileEnabled = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void TrainingForm_Load(object sender, EventArgs e)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            initialDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\..\"));

            loadImagesButton.Enabled = loadImgEnabled;
            loadTrainFileButton.Enabled = loadTrainFileEnabled;
        }

        private void AddImagesButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = initialDirectory;
                folderBrowserDialog.Description = "Выберите директорию с изображениями PNG";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedDirectory = folderBrowserDialog.SelectedPath;
                    imagePaths = Directory.GetFiles(selectedDirectory, "*.png").ToList();                   
                }
                else
                {
                    return;
                }
            }

            loadImgEnabled = false;
        }


        private void LoadTrainFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = initialDirectory;
                openFileDialog.Filter = "IDL files|*.idl";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    trainingFile = openFileDialog.FileName;
                }
                else
                    return;
            }

            loadTrainFileEnabled = false;
        }

        private async void TrainButton_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    var areas = new List<Area>();
                    var lines = File.ReadAllLines(trainingFile);
                    foreach (var line in lines)
                    {
                        var values = line.Split('\t');

                        areas.Add(new Area()
                        {
                            Num = Convert.ToInt32(values[0]),
                            X0 = Convert.ToInt32(values[2]),
                            Y0 = Convert.ToInt32(values[1]),
                        });
                    }

                    var hog = new HistogramsOfOrientedGradients();
                    var grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);

                    var trainData = new List<TrainData>();
                    foreach (var imagePath in imagePaths)
                    {
                        var fileNum = Convert.ToInt32(Path.GetFileNameWithoutExtension(imagePath));
                        var curArea = areas.FirstOrDefault(x => x.Num == fileNum);

                        if (curArea == null)
                        {
                            Console.WriteLine($"Аннотация для изображения {fileNum} не найдена.");
                            continue;
                        }

                        var originalImage = new Bitmap(imagePath);
                        var section = new Rectangle(curArea.X0, curArea.Y0, 80, 200);

                        var trueImage = originalImage.Clone(section, originalImage.PixelFormat);

                        var grayTrueImage = grayscaleFilter.Apply(trueImage);
                        var trueImageHog = hog.Transform(grayTrueImage).SelectMany(x => x.Descriptor).ToArray();

                        trainData.Add(new TrainData
                        {
                            Label = true,
                            Hog = trueImageHog.Select(x => (float)x).ToArray()
                        });

                        var falseImage = GetFirstBackgroundArea(originalImage, 80, 200, curArea.X0);
                        if (falseImage == null)
                        {
                            Console.WriteLine($"Не удалось найти область фона для изображения {fileNum}");
                            continue;
                        }
                        var grayFalseImage = grayscaleFilter.Apply(falseImage);
                        var falseImageHog = hog.Transform(grayFalseImage).SelectMany(x => x.Descriptor).ToArray();

                        trainData.Add(new TrainData
                        {
                            Label = false,
                            Hog = falseImageHog.Select(x => (float)x).ToArray()
                        });
                    }

                    var mlContext = new MLContext();
                    var trainDataView = mlContext.Data.LoadFromEnumerable(trainData);

                    var model = mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression();

                    string featuresColumnName = "Features";
                    var pipeline = mlContext.Transforms
                        .Concatenate(featuresColumnName, nameof(TrainData.Hog))
                        .Append(model);

                    var transformer = pipeline.Fit(trainDataView);

                    mlContext.Model.Save(transformer, trainDataView.Schema, $"{initialDirectory}\\model.mdl");
                });

                MessageBox.Show("Обучение закончено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка во время обучения: {ex.Message}");
            }
        }

        public static Bitmap GetFirstBackgroundArea(Bitmap image, int width, int height, int x0)
        {
            for (int x = 0; x + width <= image.Width; x += 100)
            {
                if (!IsIntersect(x, x0, width))
                {
                    var rect = new Rectangle(x, 0, width, height);
                    return image.Clone(rect, image.PixelFormat);
                }
            }

            return null;
        }

        private static bool IsIntersect(int x, int x0, int width)
        {
            return x < x0 + width && x + width > x0;
        }
    }

    public class Area
    {
        public int Num { get; set; }
        public int X0 { get; set; }
        public int Y0 { get; set; }
    }

    public class TrainData
    {
        public bool Label { get; set; }
        [VectorType(3564)]
        public float[] Hog { get; set; }
    }
}
