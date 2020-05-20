using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Amazon.Rekognition.Model;

namespace AWS_2
{
    /// <summary>
    /// Interaction logic for PictureService.xaml
    /// </summary>
    public partial class PictureService : Window
    {
        bool isChoose = false;
        public PictureService()
        {
            InitializeComponent();
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*jpg";
            openFileDialog.InitialDirectory = @"C:\Users\DELL\Pictures";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                txtFilePath.Content = $"File: {filePath}";
                pbMyPictureBox.LoadPicture(filePath);
                isChoose = true;
            }
        }

        private void OnImageModeration(object sender, RoutedEventArgs e)
        {
            if(isChoose)
            {
                DetectModerationLabelsResponse response = pbMyPictureBox.ImageModeration();
                string result = "";

                foreach (ModerationLabel label in response.ModerationLabels)
                {
                    result += $"Label: {label.Name}\n\tConfidence: {Math.Round(label.Confidence, 2)}%\n\n";
                }

                if (response.ModerationLabels.Count == 0)
                    result = "Image is Safe";

                txtResult.Text = result;
            }
            else
            {
                txtResult.Text = "Error: File Not Choose";
            }
        }

        private void OnImageToText(object sender, RoutedEventArgs e)
        {
            if (isChoose)
            {
                DetectTextResponse response = pbMyPictureBox.ImageToText();
                string result = "";

                foreach (TextDetection label in response.TextDetections)
                {
                    result += $"Text ID: {label.Id.ToString()}\n\tText: {label.DetectedText}\n\tConfidence: {Math.Round(label.Confidence, 2)}%\n\tType: {label.Type.ToString()}\n\n";
                }

                if (response.TextDetections.Count == 0)
                    result = "Text not Found";

                txtResult.Text = result;
            }
            else
            {
                txtResult.Text = "Error: File Not Choose";
            }
        }

    }
}
