using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = Amazon.Rekognition.Model.Image;
using Label = Amazon.Rekognition.Model.Label;

namespace AWS_2.User_Controls
{
    /// <summary>
    /// Interaction logic for PictureBox.xaml
    /// </summary>
    public partial class PictureBox : UserControl
    {
        string filePath = null;
        List<BindingBox> bindingBoxes = new List<BindingBox>();

        public PictureBox()
        {
            InitializeComponent();
        }

        public void LoadPicture(string tempFilePath)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(tempFilePath));
            imgPictureFrame.Source = bitmap;
            imgPictureFrame.Width = bitmap.Width;
            imgPictureFrame.Height = bitmap.Height;
            this.Height = bitmap.Height;
            this.Width = bitmap.Width;
            filePath = tempFilePath;
        }


        public DetectModerationLabelsResponse ImageModeration()
        {
            Image image = new Image();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = null;
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                image.Bytes = new MemoryStream(data);
            }

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectModerationLabelsRequest detectModerationLabelsRequest = new DetectModerationLabelsRequest()
            {
                Image = image,
                MinConfidence = 60F
            };

            DetectModerationLabelsResponse detectModerationLabelsResponse = rekognitionClient.DetectModerationLabels(detectModerationLabelsRequest);
            return detectModerationLabelsResponse;
        }

        public DetectTextResponse ImageToText()
        {
            Image image = new Image();
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] data = null;
                data = new byte[fs.Length];
                fs.Read(data, 0, (int)fs.Length);
                image.Bytes = new MemoryStream(data);
            }

            AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

            DetectTextRequest detectTextRequest = new DetectTextRequest()
            {
                Image = image,

            };

            DetectTextResponse detectTextResponse = rekognitionClient.DetectText(detectTextRequest);

            double width = imgPictureFrame.Width;
            double height = imgPictureFrame.Height;

            foreach (TextDetection text in detectTextResponse.TextDetections)
            {
                bool isLine = true;
                if (text.Type != TextTypes.LINE)
                {
                    isLine = false;
                }

                BoundingBox boundingBox = text.Geometry.BoundingBox;
                BindingBox bindingBox = new BindingBox(width * boundingBox.Width, height * boundingBox.Height, height * boundingBox.Top, width * boundingBox.Left, text.Id.ToString(), isLine);
                gContainer.Children.Add(bindingBox);
                bindingBoxes.Add(bindingBox);
            }

            return detectTextResponse;
        }

        public void ResetPictureBox()
        {
            if (bindingBoxes.Count>0)
            {
                foreach(BindingBox item in bindingBoxes)
                {
                    gContainer.Children.Remove(item);
                }

                bindingBoxes.Clear();
            }
        }

        
    }
}
