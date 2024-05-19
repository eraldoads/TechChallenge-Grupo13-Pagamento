# TechChallenge-Grupo13-Pagamento
Este repositório é dedicado ao microsserviço de pagamento, o qual foi desmembrado do monolito criado para a lanchonete durante a evolução da pós-graduação em Arquitetura de Software da FIAP.

Tanto o build e push para o repositório no ECR da AWS usando Terraform, quanto a análise de código e cobertura de testes utilizando SonarCloud são realizados via Github Actions.

## 🖥️ Grupo 13 - Integrantes
🧑🏻‍💻 *<b>RM352133</b>*: Eduardo de Jesus Coruja </br>
🧑🏻‍💻 *<b>RM352316</b>*: Eraldo Antonio Rodrigues </br>
🧑🏻‍💻 *<b>RM352032</b>*: Luís Felipe Amengual Tatsch </br>

## Arquitetura
Quando disparamos a Github Action, é realizado o build da aplicação e o push para o repositório criado previamente no Elastic Container Registry (ECS).
Ao final da action, é atualizada a Service no Elastic Container Service (ECS), executando assim a service que irá realizar a criação do container.

![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/47857203/d50cec6d-e2da-439b-aaad-03b6ec0e90a5)

Para este microsserviço, utilizamos .NET 8.0, o que também representa uma evolução de tecnologia em relação ao monolito, o qual foi baseado no .NET 6.0 .

## Testes

Utilizamos a ferramenta SonarCloud para análise de código e cobertura de testes. Para este microsserviço, atingimos 100% de cobertura, conforme abaixo:

https://sonarcloud.io/summary/overall?id=eraldoads_TechChallenge-Grupo13-Pagamento

![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/47857203/1029386d-0f98-4274-9d2a-c5f3c51b10f2)

### BDD – Behavior-Driven Development
Para organizar o código e armazenar os cenários de testes com a técnica BDD (Desenvolvimento Orientado por Comportamento) criou-se um projeto baseado na ferramenta SpecFlow que trata-se de uma implementação oficial do Cucumber para .NET que utiliza o Gherkin como analisador. Neste exemplo foi configurado em conjunto com o framework nUnit. 

#### Organização do Teste
Um novo projeto, chamado <b>PagamentoTestBDD</b> foi adicionado à solução na pasta BDD dentro da estrutura de Testes.
Arquivo de configuração do projeto PagamentoTestBDD
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/fcdc5f29-5709-4769-98f7-b5cb326b3e61)
 
O arquivo <b>Pagamento.feature</b> armazena as funcionalidades (features) que serão utilizadas como base para a validação da API Pagamento. Para efeito de estudo, foi definido o cenário que realiza a busca de um registro de Pagamento a partir de um ID de um Pedido informado para validar que seu status é Pendente.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/b77b96bb-6cb9-4585-98ad-0e68cb6f5745)
 
O arquivo <b>GerenciarPagamentosStepDefinitions.cs</b> contém os passos que serão executados para validar os cenários definidos nas features. No exemplo, há três métodos implementados para validar o cenário.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/f694465b-0563-4ddd-a824-7d81d60e9fc2)
 
##### GivenQueUmPagamentoJaFoiRegistradoParaOPedidoComID
Implementa os passos que serão realizados para atender o que foi estabelecido no “Dado”.
Neste exemplo o método é responsável por validar que o ID do Pedido e o Status de Pagamento a verificar são válidos.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/8687a317-1257-476e-91e2-a41109272eb0)
 
##### WhenRequisitarABuscaDoStatusDoPagamentoPorSeuIDAsync
Implementa os passos que serão realizados para atender o que foi estabelecido no “Quando”.
Neste exemplo o método realiza a busca pelo Pedido utilizando o seu ID.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/168529f1-4404-49d0-9b85-65a34794c355)
 
##### ThenOStatusDoPagamentoEExibidoComSucesso
Implementa os passos que serão realizados para atender o que foi estabelecido no “Então”.
Neste exemplo o método confere se o Status do Pagamento exibido é o esperado.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/3e411563-cd57-4765-a80e-9e1d35030c2c)
 
#### Execução dos Testes
A imagem a seguir apresenta o resultado da execução de todos os testes que a solução possui, destacando o cenário definido em BDD PagamentoTestBDD, bem como o detalhe dos passos realizados.
![image](https://github.com/eraldoads/TechChallenge-Grupo13-Pagamento/assets/149120484/9cd4bd09-9865-44a5-bff6-b2ade3b4210d)
 
