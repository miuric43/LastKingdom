using System.Reflection;
using System.IO;
using System.Resources;
using System.Media;
using System.Diagnostics;
using System;
namespace LKCamelot
{
    public partial class Sound
    {
        public Sound()
        {
            InitializeComponent();
        }

        public void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        public void Sound_Load()
        {
            Assembly assembly;
            Stream soundStream = (Properties.Resources._0);
            SoundPlayer sp;
            assembly = Assembly.GetExecutingAssembly();
            sp = new SoundPlayer(soundStream);
            sp.Play();
        }
    }
}