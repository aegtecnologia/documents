using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Selenium.Teste
{
    public enum MensagemStatus
    {
        PENDENTE,
        ENVIADA,
        NAOENCONTRADO
    }
    public class WhatsappService
    {
        const string urlBase = "https://web.whatsapp.com/";
        IWebDriver driver = null;
        public WhatsappService()
        {
            AbrirNavegador();
        }

        private void AbrirNavegador()
        {
            driver = new ChromeDriver(Environment.CurrentDirectory);
        }
        private void FecharNavegador()
        {
            if (driver != null) driver.Close();
        }

        public List<Mensagem> EnviarMensagens(string mensagem, List<Contato> contatos)
        {
            var retorno = new List<Mensagem>();

            try
            {
                driver.Navigate().GoToUrl(urlBase);

                //Thread.Sleep(5000);

                AguardarLogin();

                foreach (var contato in contatos)
                {
                    var msg = new Mensagem()
                    {
                        Conteudo = mensagem,
                        Destinatario = contato,
                        Status = MensagemStatus.PENDENTE
                    };

                    EnviarMensagem(ref msg);
                    retorno.Add(msg);
                }
            }
            catch (Exception erro)
            {
                Console.WriteLine($"Erro: {erro.Message}");
            }
            finally
            {
                FecharNavegador();
            }

            return retorno;

        }

        private void AguardarLogin()
        {
            try
            {
                while (!IsLoggedIn()) ;

                //var elemento = new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                //    .Until(ExpectedConditions.ElementExists(By.Id("_2rZZg")));
            }
            catch
            {
                throw new Exception("Login não efetuado!");
            }
        }

        private bool IsLoggedIn()
        {
            bool flag = false;
            try
            {
                flag = driver.FindElement(By.ClassName("_2rZZg")).Displayed;
            }
            catch { flag = false; }
            return flag;
        }
        private void EnviarMensagem(ref Mensagem mensagem)
        {
            try
            {
                var url = $"https://api.whatsapp.com/send?phone={mensagem.Destinatario.Telefone}";

                mensagem.DataInicio = DateTime.Now;

                driver.Navigate().GoToUrl(url);

                var btnEnviar = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementExists(By.Id("action-button")));

                btnEnviar.Click();

                var inputMensagem = new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementExists(By.ClassName("_3u328")));

                inputMensagem.SendKeys(mensagem.Conteudo);
                inputMensagem.SendKeys(Keys.Enter);

                Thread.Sleep(3000);

                mensagem.DataFim = DateTime.Now;
                mensagem.Status = MensagemStatus.ENVIADA;
            }

            catch (Exception erro)
            {
                mensagem.DataFim = DateTime.Now;
                mensagem.Status = MensagemStatus.NAOENCONTRADO;
            }

        }
    }
    public class Contato
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
    }
    public class Mensagem
    {
        public Contato Destinatario { get; set; }
        public string Conteudo { get; set; }
        public MensagemStatus Status { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
