using Ghostscript.NET;
using Ghostscript.NET.Processor;
using System.Collections.Generic;
using System;

namespace CompactarPdf
{
    public class CompressPdf
    {
        private GhostscriptVersionInfo _gs_verssion_info;
        private string _inputFile;
        private string _outputFile;

        public string LIB
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                    return Environment.CurrentDirectory + "\\gs\\gs9.22-64\\bin;" +
                           Environment.CurrentDirectory + "\\gs\\gs9.22-64\\lib;" +
                           Environment.CurrentDirectory + "\\gs\\gs9.22-64\\fonts";
                else
                    return Environment.CurrentDirectory + "\\gs\\gs9.22-32\\bin;" +
                           Environment.CurrentDirectory + "\\gs\\gs9.22-32\\lib;" +
                           Environment.CurrentDirectory + "\\gs\\gs9.22-32\\fonts";
            }
        }

        public string DLL
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                    return Environment.CurrentDirectory + "\\gs\\gs9.22-64\\bin\\gsdll64.dll";
                else
                    return Environment.CurrentDirectory + "\\gs\\gs9.22-32\\bin\\gsdll32.dll";
            }
        }
        
        public CompressPdf(string nameFile, string newNameFile)
        {
            this._inputFile = nameFile;
            this._outputFile = newNameFile;
            this._gs_verssion_info = new GhostscriptVersionInfo(new System.Version("9.22"), DLL, LIB, GhostscriptLicense.GPL);
        }

        public void ProcessFiles()
        {
            CompressDocument();
        }

        private void CompressDocument()
        {
            List<string> gsArgs = new List<string>();

            gsArgs.Add("-empty");
            gsArgs.Add("-dSAFER");
            gsArgs.Add("-dBATCH");
            gsArgs.Add("-dNOPAUSE");
            gsArgs.Add("-dNOPROMPT");

            gsArgs.Add("-sDEVICE=pdfwrite");
            gsArgs.Add("-dCompatibilityLevel=1.4");
            // dPDFSETTINGS is basically our commpression option
            //  - /screen selects low-resolution output similar to the Acrobat Distiller "Screen Optimized" setting.
            //  - /ebook selects medium-resolution output similar to the Acrobat Distiller "eBook" setting.
            //  - /printer selects output similar to the Acrobat Distiller "Print Optimized" setting.
            //  - /prepress selects output similar to Acrobat Distiller "Prepress Optimized" setting.
            //  - /default selects output intended to be useful across a wide variety of uses, possibly at the expense of a larger output file.
            // gsArgs.Add("-dPDFSETTINGS=/screen");
            gsArgs.Add("-dPDFSETTINGS=/ebook");

            gsArgs.Add("-sOutputFile=" + _outputFile + "");
            gsArgs.Add("-f");
            gsArgs.Add(_inputFile);

            using (GhostscriptProcessor processor = new GhostscriptProcessor(_gs_verssion_info, true))
                processor.StartProcessing(gsArgs.ToArray(), null);
        }
    }
}
