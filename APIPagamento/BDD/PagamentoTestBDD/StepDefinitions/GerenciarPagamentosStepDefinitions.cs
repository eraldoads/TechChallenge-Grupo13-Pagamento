using System;
using TechTalk.SpecFlow;
using NUnit.Framework;
using Domain.Entities;
using Newtonsoft.Json;

namespace PagamentoTestBDD.StepDefinitions
{
    [Binding]
    public class GerenciarPagamentosStepDefinitions
    {
        private String ENDPOINT_PAGAMENTO = "http://localhost:5245/pagamento";

        [Given(@"que o Pedido com ID (.*) está com o pagamento(.*)")]
        
        public void GivenQueUmPagamentoJaFoiRegistradoParaOPedidoComID(int idPedido, string statusPagamento)
        {
            // armazena o número do Pedido no contexto
            ScenarioContext.Current["idPedido"] = idPedido;

            // armazena o Status esperado no contexto
            ScenarioContext.Current["statusPagamento"] = statusPagamento.Trim();

            // verifica se o id do Pedido informado é maior que zero
            Assert.True(idPedido > 0);
                        
            // verifica se o status informado é válido
                        
            Assert.True(ScenarioContext.Current["statusPagamento"].ToString().Equals("Pendente"));

        }

        [When(@"requisitar a busca do Status do Pagamento por seu ID")]
        public async Task WhenRequisitarABuscaDoStatusDoPagamentoPorSeuIDAsync()
        {
            var client = new HttpClient();

            // inicializa a variável com o id do Pedido para realizar a busca
            var id = ScenarioContext.Current["idPedido"];

            // monta o endpoint que será utilizado no GET, incluindo o parâmetro
            var request = new HttpRequestMessage(HttpMethod.Get, ENDPOINT_PAGAMENTO + "/" + id + "/status");

            // realiza o GET
            var response = await client.SendAsync(request);

            // converte o retorno para Pagamento
            var rtn = JsonConvert.DeserializeObject<Pagamento>(await response.Content.ReadAsStringAsync());

            // armazena no contexto o conteúdo retornado para utilizar posteriormente na validação
            ScenarioContext.Current["Pagamento"] = rtn;

            // verifica se o Status do retorno da requisição é o esperado (200)
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Then(@"o Status do Pagamento é exibido com sucesso")]
        public void ThenOStatusDoPagamentoEExibidoComSucesso()
        {
            // inicializa a variável com o Pagamento retornado na consulta anterior
            var pagamentoRetornado = (Pagamento)ScenarioContext.Current["Pagamento"];
                        
            // inicializa a variável com o Status informado no início
            var statusInicial = (string)ScenarioContext.Current["statusPagamento"];

            // valida se o Status retornado é igual ao Status indicado no início
            Assert.True(pagamentoRetornado.StatusPagamento == statusInicial);
        }
    }
}
