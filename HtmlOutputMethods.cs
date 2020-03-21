using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace MatrixCalculus
{
    public class HtmlOutputMethods
    {
        public static string ToHtmlWithMathJax(string Latex)
        {
            string sGraph = @"
<!DOCTYPE HTML>
<html>
    <head>
        <title>Equation</title>
        <meta charset=""utf-8"" />
        <script type='text/javascript' async src = 'https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML'></script>
        <script type='text/x-mathjax-config'>
            MathJax.Hub.Register.StartupHook('End', function()
            {
                var x = document.getElementById('pLatex');
                x.style.display = 'block';
            });
        </script>
    </head>
    <body>
        <p id='pLatex' style='height: 300px; width: 300px; display: none;'>
            \begin{equation}
           REPLACE_ME_WITH_TEXT
           \end{equation}     
        </p>
    </body>
</html>";
            return sGraph.Replace("REPLACE_ME_WITH_TEXT", Latex);
        }

        public static string ToHtmlVectorGraph2D(RealVector rv)
        {
            string ret = "";
            string Ds3 = @"
<!DOCTYPE HTML>
<html>
    <head>
        <title>Vector Graph 2D</title>
        <meta charset=""utf-8"" />
    </head>
    <body>
        <div id='Area'></div>
        <script type='text/javascript' src='https://d3js.org/d3.v4.js'></script>
        <script>
            // set the dimensions and margins of the graph
            var margin = {top: 10, right: 40, bottom: 30, left: 30},
                width = 450 - margin.left - margin.right,
                height = 400 - margin.top - margin.bottom;

            // append the svg object to the body of the page
            var sVg = d3.select('#Area')
              .append('svg')
                .attr('width', width + margin.left + margin.right)
                .attr('height', height + margin.top + margin.bottom)

              // translate this svg element to leave some margin.
              .append('g')
                .attr('transform', 'translate(' + margin.left + ',' + margin.top + ')');

            // X scale and Axis
            var x = d3.scaleLinear()
                .domain([0, {REPLACE_X])    // This is the min and the max of the data: 0 to 100 if percentages
                .range([0, width]);         // This is the corresponding value I want in pixels
            sVg
              .append('g')
              .attr('transform', 'translate(0,' + height + ')')
              .call(d3.axisBottom(x));

            // X scale and Axis
            var y = d3.scaleLinear()
                .domain([0, REPLACE_Y])     // This is the min and the max of the data: 0 to 100 if percentages
                .range([height, 0]);        // This is the corresponding value I want in pixels
            sVg
              .append('g')
              .call(d3.axisLeft(y));

        </script>
    </body>
</html>";

            int scaleX = (int)rv[0] + (int)(rv[0] / 10);
            int scaleY = (int)rv[1] + (int)(rv[1] / 10);
            
            return Ds3.Replace("REPLACE_X", scaleX.ToString()).Replace("REPLACE_Y", scaleY.ToString());
        }

        public static string ToHtmlWithMathJaxInline(string Latex)
        {
            string sGraph = @"
<!DOCTYPE HTML>
<html>
    <head>
        <title>Inline</title>
        <meta charset=""utf-8"" />
        <script type='text/javascript' async src = 'https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML'></script>

        <script type='text/x-mathjax-config'>
            MathJax.Hub.Register.StartupHook('End', function()
            {
                var x = document.getElementById('pLatex');
                x.style.display = 'block';
            });
        </script>

    </head>
    <body>
        <p id='pLatex' style='height: 300px; width: 300px; display:none;'>
            $$
            REPLACE_ME_WITH_TEXT
            $$     
        </p>
    </body>
</html>";

            return sGraph.Replace("REPLACE_ME_WITH_TEXT", Latex);
        }

        public static string ToHtmlWithMathJaxEquation(string Latex)
        {
            string sGraph = @"
<!DOCTYPE HTML>
<html>
    <head>
        <title>Equation</title>
        <meta charset=""utf-8"" />
        <script type='text/javascript' async src = 'https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML'></script>
        <script type='text/x-mathjax-config'>
            MathJax.Hub.Register.StartupHook('End', function()
            {
                var x = document.getElementById('pLatex');
                x.style.display = 'block';
            });
        </script>
    </head>
    <body>
        <p id='pLatex' style='height: 300px; width: 300px; display:none;'>
            \begin{equation}      
            REPLACE_ME_WITH_TEXT
            \end{equation}      
        </p>
    </body>
</html>";
            return sGraph.Replace("REPLACE_ME_WITH_TEXT", Latex);
        }

        public static string ToHtmlWithMathJaxEquationText(string Latex, string Text)
        {
            string sGraph = @"
<!DOCTYPE HTML>
<html>
    <head>
        <title>Equation Text</title>
        <meta charset=""utf-8"" />
        <script type='text/javascript' async src = 'https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML'></script>
        <script type='text/x-mathjax-config'>
            MathJax.Hub.Register.StartupHook('End', function()
            {
                var x = document.getElementById('pLatex');
                x.style.display = 'block';
            });
        </script>

    </head>
    <body>
        <p>
        REPLACE_ME_WITH_TEXT
        </p>
        <p id='pLatex' style='height: 300px; width: 300px; display:none;'>
            \begin{equation}      
                   REPLACE_ME_WITH_LATEX
            \end{equation}      
        </p>
    </body>
</html>";
            string retVal = sGraph.Replace("REPLACE_ME_WITH_LATEX", Latex).Replace("REPLACE_ME_WITH_TEXT", Text);
            return retVal;
        }

        /// <summary>
        /// Launches the default system browser.  Supports Windows, Linux and Mac OSX.
        /// </summary>
        /// <param name="url"></param>
        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw new System.Exception("Unsupported operating system.");
            }
        }

        public static void Write2DVectorToHtmlAndLaunch(RealVector rv, string FileName)
        {
            string path = Directory.GetCurrentDirectory();
            string fil = Path.Combine(path, FileName);
            string text = HtmlOutputMethods.ToHtmlVectorGraph2D(rv);

            File.WriteAllText(fil, text);
            OpenBrowser(fil);
        }

        public static void WriteLatexToHtmlAndLaunch(string Latex, string FileName)
        {
            string path = Directory.GetCurrentDirectory() + "/html/";
            string fil = Path.Combine(path, FileName);
            string text = HtmlOutputMethods.ToHtmlWithMathJaxInline(Latex);

            File.WriteAllText(fil, text);
            OpenBrowser(fil);
        }

        public static void WriteLatexEqToHtmlAndLaunch(string Latex, string FileName)
        {
            string path = Directory.GetCurrentDirectory() + "/html/";
            string fil = Path.Combine(path, FileName);
            string text = HtmlOutputMethods.ToHtmlWithMathJaxEquation(Latex);

            File.WriteAllText(fil, text);
            OpenBrowser(fil);
        }
    }
}