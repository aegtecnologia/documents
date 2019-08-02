using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Xml;

namespace ConsoleInterage
{
    public class Jabur
    {
        static string wsUrl = "http://webservice.onixsat.com.br";
        static string usuario = "04900055000109";
        static string senha = "GV@2792!";


        static string ReceberMensagensMock()
        {
            var pasta = @"C:\AEG\Desenvolvimento\Projetos\Interage\Arquivos\";
            var arquivo = $"{pasta}jabur-mensagens-20190708171146.xml";

            using (StreamReader sr = new StreamReader(arquivo))
            {
                return sr.ReadToEnd();
            }
        }
        static List<Posicao> ListarPosicoes(DataTable dt)
        {
            var posicoes = new List<Posicao>();

            foreach (DataRow dr in dt.Rows)
            {
                posicoes.Add(new Posicao()
                {
                    VeiculoId = Convert.ToInt32(dr["veiID"]),
                    Data = Convert.ToDateTime(dr["dt"]),
                    Latitude = Convert.ToString(dr["lat"]),
                    Longitude = Convert.ToString(dr["lon"]),
                    Cidade = Convert.ToString(dr["mun"]),
                    Estado = Convert.ToString(dr["uf"]),
                    Endereco = Convert.ToString(dr["rua"]),
                });
            }

            return posicoes;
        }

        public static List<Posicao> ListarPosicoes()
        {
            var posicoes = new List<Posicao>();
            var xmlResponse = ReceberMensagensMock();

            using (MemoryStream ms = new MemoryStream())
            {
                var xml = new XmlDocument();
                xml.LoadXml(xmlResponse);
                var mensagens = xml.GetElementsByTagName("MensagemCB");
                foreach (XmlNode no in mensagens)
                {
                    string sJson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(no,Newtonsoft.Json.Formatting.None,true);
                    dynamic msg = Newtonsoft.Json.JsonConvert.DeserializeObject(sJson);
                    posicoes.Add(new Posicao()
                    {
                        VeiculoId = (int)msg.veiID,
                        Data = msg.dt,
                        Latitude = msg.lat,
                        Longitude = msg.lon,
                        Cidade = msg.mun,
                        Estado = msg.uf,
                        Endereco = msg.rua,

                    });
                }
                //Newtonsoft.Json.JsonConvert.SerializeXmlNode()
                //xml.Save(ms);
                //DataSet ds = new DataSet();
                //ds.ReadXml(ms);

                //posicoes = ListarPosicoes(ds.Tables[0]);
            }

            return posicoes;
        }

        public static string ReceberMensagens()
        {
            string id = "46062233155";

            string comando = @"<RequestMensagemCB>
<login>" + usuario + @"</login>
<senha>" + senha + @"</senha>
<mId>" + id + @"</mId>
</RequestMensagemCB>";

            string retorno = RequestXml(comando);
            return retorno;
        }
        public static string EnviarComando()
        {
            /*
             Descrição:
id: é um identificador seqüencial para o envio de comandos aos veículos, este valor nunca
deve ser repetido, pois caso isto ocorra, ficará comprometido o status do comando;
veiID: identificação do veículo;
mensagem: código do comando. Ao enviar um comando é necessário saber o tipo de
equipamento, informação disponível no campo eqp, quando solicitada as informações dos
veículos.
usuario: usuário que está enviando o comando;
             */

            string cmd = @"<RequestEnvioComando login='" + usuario + "' senha='" + senha + @"'>
<comando>
<id>?</id>
<veiID>?</veiID>
<mensagem>?</mensagem>
<usuario>?</usuario>
</comando>
</RequestEnvioComando>";

            return cmd;
        }
        public static string ListarVeiculos()
        {
            // obter lista de veiculos
            string requestListaVeiculos = @"<RequestVeiculo>
<login>" + usuario + @"</login>
<senha>" + senha + @"</senha>
</RequestVeiculo>";

            string retorno = RequestXml(requestListaVeiculos);
            return retorno;

        }

        static byte[] Decompress(byte[] data)
        {
            //
            try
            {
                MemoryStream input = new MemoryStream();
                input.Write(data, 0, data.Length);
                input.Position = 0;
                GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true);
                byte[] buff = new byte[256];
                MemoryStream output = new MemoryStream();
                int read = gzip.Read(buff, 0, buff.Length);
                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }
                gzip.Close();
                byte[] buffer = output.ToArray();
                output.Dispose();
                return buffer;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ZipLibraryException("Falha ao descompactar arquivo no formato .
                //gzip", ex);
            }

            /*if (IsValidDecompress(data))
            {
               
            }
            return null;*/
        }
        static HttpWebRequest CreateRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://webservice.onixsat.com.br");
            request.Method = "POST";
            request.ContentType = "text/xml";
            return request;
        }
        static string RequestXml(string strRequest)
        {
            string result = string.Empty;
            try
            {
                // requisição xml em bytes

                byte[] sendData = UTF8Encoding.UTF8.GetBytes(strRequest);
                // cria requisicao
                HttpWebRequest request = CreateRequest();
                Stream requestStream = request.GetRequestStream();
                // envia requisição
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Flush();
                requestStream.Dispose();
                // captura resposta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                MemoryStream output = new MemoryStream();
                byte[] buffer = new byte[256];
                int byteReceived = -1;
                do
                {
                    byteReceived = responseStream.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, byteReceived);
                } while (byteReceived > 0);
                responseStream.Dispose();
                response.Close();
                buffer = output.ToArray();
                output.Dispose();
                // transforma resposta em string para leitura xml
                result = UTF8Encoding.UTF8.GetString(Decompress(buffer));
            }
            catch (Exception ex)
            {
                // tratar exceção
            }
            return result;
        }

    }
}

