﻿using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;

namespace QRBarcodeReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo device in filterInfoCollection)
            {
                cboCamera.Items.Add(device.Name);
            }
            cboCamera.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if(result != null)
            {
                txtBarCode.Invoke(new MethodInvoker(delegate ()
                {
                    txtBarCode.Text = result.ToString();
                }));
            }

            pictureBox.Image = bitmap;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(videoCaptureDevice != null)
            {
                if(videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                }
            }

        }
    }
}
