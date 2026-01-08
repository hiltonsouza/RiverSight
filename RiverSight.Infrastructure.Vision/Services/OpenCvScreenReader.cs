using OpenCvSharp;
using System.Drawing;
using RiverSight.Domain.Entities;
using RiverSight.Domain.Interfaces;
using OpenCvSharp.Extensions;


namespace RiverSight.Infrastructure.Vision.Services
{
    public class OpenCvScreenReader : IScreenReader, IDisposable
    {
        private readonly Dictionary<string, Mat> _templates = new();
        private const double MatchThreshold = 0.85; // 85% de precisão mínima

        //Áreas de interesse ((Hardcoded pro MVP, depois vem de config)
        //Ajuste esses valores conforme o seu emulador!
        private readonly Rectangle _heroCard1Rect = new Rectangle(350, 480, 40, 55);
        private readonly Rectangle _heroCard2Rect = new Rectangle(400, 480, 40, 55);

        public OpenCvScreenReader(string assetsPath)
        {
            LoadTemplates(assetsPath);
        }

        private void LoadTemplates(string path)
        {
            var files = Directory.GetFiles(path, "*.png");
            foreach (var file in files)
            {
                var key = Path.GetFileNameWithoutExtension(file); // "Ah", 'Ks', etc.
                // Carrega em escala de cinza ( mais rápido e ignora variações de cor/brilho)
                var mat = Cv2.ImRead(file, ImreadModes.Grayscale);
                _templates[key] = mat;
            }
        }

        //Implementação da interface do Domínio
        public IEnumerable<Card> ReadHeroHands()
        {
            var cards = new List<Card>();

            // Captura e tenta identificar Carta 1
            var c1 = IdentifyCardInRegion(_heroCard1Rect);
            if (c1 != null) cards.Add(c1);

            //Captura e tenta identificar Carta 2
            var c2 = IdentifyCardInRegion(_heroCard2Rect);
            if (c2 != null) cards.Add(c2);

            return cards;
        }

        public IEnumerable<Card> ReadCommunityCards()
        {
            // TODO: implementar a lógica para flop/turn/river
            // segue a mesma lógica de HeroCards, só mudam as coordenadas

            return new List<Card>();
        }

        // O "Cérebro" do Template Matching
        private Card? IdentifyCardInRegion(Rectangle region)
        {
            // 1. Captura a tela (Usando System.Drawing por simplicidade)
            using var screenBmp = CaptureScreen(region);

            // 2. Converte para formato OpenCv
            using var sourceMat = BitmapConverter.ToMat(screenBmp);
            using var sourceGray = new Mat();
            Cv2.CvtColor(sourceMat, sourceGray, ColorConversionCodes.BGR2GRAY);

            double bestScore = 0;
            string bestMatchKey = "";

            // 3. Compara com todos os 52 templates
            foreach (var template in _templates)
            {
                using var result = new Mat();
                Cv2.MatchTemplate(sourceGray, template.Value, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

                if(maxVal > bestScore)
                {
                    bestScore = maxVal;
                    bestMatchKey = template.Key;
                }
            }
            // 4. Valida e converte
            if(bestScore >= MatchThreshold)
            {
                // O Helper transforma "Ah" em new Card(Rank.Ace, Suit.Hearts)
                return CardParser.Parse(bestMatchKey);
            }

            return null; // Nenhuma carta encontrada (mesa vazia ou carta virada)
        }

        private Bitmap CaptureScreen(Rectangle rect)
        {
            var bmp = new Bitmap(rect.Width, rect.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            }
            return bmp;
        }

        public void Dispose()
        {
            // Limpa a memória não gerenciada do OpenCv
            foreach (var mat in _templates.Values)
            {
                mat.Dispose();
            }
        }
    }
}
