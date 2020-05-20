﻿using System;
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
using Amazon.Translate;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Translate.Model;
using Amazon;
using System.IO;

namespace AWS_2
{
    /// <summary>
    /// Interaction logic for TextService.xaml
    /// </summary>
    public partial class TextService : Window
    {
        string AWS_ACCESS_KEY = "AKIASASJUZ22FVGPJRFH";
        string AWS_SECRET_KEY = "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";
        bool isFolderExist = false;

        public TextService()
        {
            InitializeComponent();
        }

        private void OnTranslate(object sender, RoutedEventArgs e)
        {
            string translateText = txtTextInput.Text.Trim();

            if (translateText != "")
            {
                var translate = new AmazonTranslateClient(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.APSoutheast1);
                var request = new TranslateTextRequest()
                {
                    Text = translateText,
                    SourceLanguageCode = "en",
                    TargetLanguageCode = "vi"
                };
                TranslateTextResponse translateResponse = translate.TranslateText(request);
                txtTextResult.Text = translateResponse.TranslatedText;
            }
           
        }

        private void OnTextToSpeech(object sender, RoutedEventArgs e)
        {
            string readingText = txtTextInput.Text.Trim();

            if (readingText != "")
            {
                var speech = new AmazonPollyClient(AWS_ACCESS_KEY, AWS_SECRET_KEY, RegionEndpoint.APSoutheast1);
                var request = new SynthesizeSpeechRequest()
                {
                    Text = readingText,
                    LanguageCode = LanguageCode.EnUS,
                    OutputFormat = OutputFormat.Mp3,
                    VoiceId = VoiceId.Salli
                };
                SynthesizeSpeechResponse speechResponse = speech.SynthesizeSpeech(request);
                string baseDicrectory = AppDomain.CurrentDomain.BaseDirectory;
                Stream stream = speechResponse.AudioStream;
                string folderPath = $"{baseDicrectory}\\TextToSpeech";
                if (!Directory.Exists(folderPath) || isFolderExist)
                {
                    Directory.CreateDirectory(folderPath);
                    isFolderExist = true;
                }

                string filePath = $"{folderPath}\\{DateTime.Now.Ticks}.Mp3";

                FileStream fileStream = File.Create(filePath);

                stream.CopyTo(fileStream);
                fileStream.Close();
                

                MediaPlayer mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri(filePath));
                mediaPlayer.Play();
            }
        }
    }
}
