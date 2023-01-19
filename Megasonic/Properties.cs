using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Megasonic
{
    public partial class Form1 : Form
    {
        // TODO
        // instead of just setting true, set false if new text is blank?

        string ImageFile
        {
            get { return imageButton.Text; }
            set
            {
                imageButton.Text = value;
                videoConditions.ImageSelected = true;
                videoPreviewConditions.ImageSelected = true;
            }
        }

        string SoundFile
        {
            get { return soundButton.Text; }
            set
            {
                soundButton.Text = value;
                soundAnalyzeConditions.SoundSelected = true;
            }
        }

        string LineFile
        {
            get { return lineButton.Text; }
            set
            {
                lineButton.Text = value;
                videoConditions.LineSelected = true;
                videoPreviewConditions.LineSelected = true;
            }
        }
    }
}
