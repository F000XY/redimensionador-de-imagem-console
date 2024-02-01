using System;
using System.Threading;
using System.IO;
using System.Drawing;
namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Redimensionador");

            Thread thread = new Thread(Redimensionar);
            thread.Start();

            Console.Read();


        }

        static void Redimensionar()
        {

            #region "Diretorios"
            string diretorioEntrada = "C:\\Users\\55119\\source\\repos\\didaticos.redimensionador\\didaticos.redimensionador\\arquivosEntrada";
            string diretorioRedimensionado = "C:\\Users\\55119\\source\\repos\\didaticos.redimensionador\\didaticos.redimensionador\\arquivosFinalizados";
            string diretorioFinalizado = "C:\\Users\\55119\\source\\repos\\didaticos.redimensionador\\didaticos.redimensionador\\arquivosRedimensionados";

            if (!Directory.Exists(diretorioEntrada))
            {
                Directory.CreateDirectory(diretorioEntrada);

            }

            if (!Directory.Exists(diretorioRedimensionado))
            {
                Directory.CreateDirectory(diretorioRedimensionado);
            }

            if (!Directory.Exists(diretorioFinalizado))
            {
                Directory.CreateDirectory(diretorioFinalizado);
            }
            #endregion

            FileInfo fileInfo;
            string caminhoRedimensionado;
            string caminhoFinalizado;
            //para evitar a criação de novaas variaveis cada vez que o loop corre, isso evita com que o 
            //programa fique mto pesado
            while (true)
            {
                //progrma vai olhar para pasta de entrada
                //se tiver arquivo ira redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorioEntrada);

                //ler o tamanho que vai redimencionar
                int tamanho = 200;



                foreach (var arquivo in arquivosEntrada)
                {
                    // Obter informações do arquivo
                    fileInfo = new FileInfo(arquivo);

                    // Construir caminho para o diretório de redimensionamento
                    caminhoRedimensionado = Path.Combine(diretorioRedimensionado, fileInfo.Name);

                    // Certificar-se de que o diretório de redimensionamento existe
                    if (!Directory.Exists(diretorioRedimensionado))
                    {
                        Directory.CreateDirectory(diretorioRedimensionado);
                    }

                    // Redimensionar e salvar a imagem
                    using (FileStream fs = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Redimensionador(Image.FromStream(fs), tamanho, caminhoRedimensionado);
                        fs.Close();
                    }

                    // Construir caminho para o diretório finalizado
                    caminhoFinalizado = Path.Combine(diretorioFinalizado, fileInfo.Name);

                    // Mover o arquivo para o diretório finalizado
                    fileInfo.MoveTo(caminhoFinalizado);
                }

                Thread.Sleep(new TimeSpan(0, 0, 1));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">imagem para ser redimencionada</param>
        /// <param name="altura">altura que desejamos redimencionar</param>
        /// <param name="caminho">caminho onde iremos grvar redimencionar</param>
        /// <returns></returns>
        /// 
        
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            //ratio redimenciona crescendo a img proporcional p cima e p baixo
            double ratio = (double)altura / imagem.Height;
            //esse (double) converte implicitamente
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);
            using (Graphics g = Graphics.FromImage(novaImagem))
            {

                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);

            }
            novaImagem.Save(caminho);
            imagem.Dispose();

        }

    }


}