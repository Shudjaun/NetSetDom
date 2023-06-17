using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSetDom.Model
{
    public class Session
    {
        private const string FILENAME = "session.xml";
        private static string _applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _applicationPath = Path.Combine(_applicationDataPath, "NetSetDom");
        /// <summary>
        /// Emplacement du fichier session.xml dans le répertoire AppData de l'utilisateur
        /// </summary>
        public string FileName { get; } = Path.Combine(_applicationPath, FILENAME);
        /// <summary>
        /// Index du dernier onglet actif à la fermeture
        /// </summary>
        public int ActiveIndex { get; set; } = 0;

        public Session() { }

    }
}
