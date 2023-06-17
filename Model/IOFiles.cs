using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSetDom.Model
{
    public class IOFiles
    {
        /// <summary>
        /// Chemin du fichier de session
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Nom du fichier de session avec l'extension
        /// </summary>
        public string SafeFileName { get; set; }

        public IOFiles() { }
        public IOFiles(string fileName)
        {
            FileName = fileName;
            SafeFileName = Path.GetFileName(fileName);
        }

    }
}
