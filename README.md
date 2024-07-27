# TechChallenge-Grupo13-Pagamento
Este repositório é dedicado ao microsserviço de pagamento, o qual foi desmembrado do monolito criado para a lanchonete durante a evolução da pós-graduação em Arquitetura de Software da FIAP.

Tanto o build e push para o repositório no ECR da AWS usando Terraform, quanto a análise de código e cobertura de testes utilizando SonarCloud são realizados via Github Actions.

## 🖥️ Grupo 13 - Integrantes
🧑🏻‍💻 *<b>RM352133</b>*: Eduardo de Jesus Coruja </br>
🧑🏻‍💻 *<b>RM352316</b>*: Eraldo Antonio Rodrigues </br>
🧑🏻‍💻 *<b>RM352032</b>*: Luís Felipe Amengual Tatsch </br>

## Saga
Na fase 5, evoluímos o nosso sistema e passamos a utilizar o padrão SAGA, no qual a comunicação entre os microsserviços ocorre por meio de mensageria.

Optamos pelo padrão de Saga Coreografada, pois, o fluxo é simples e não há necessidade de uma orquestração mais elaborada.

O Processo inicia no momento da criação do Pedido, onde é realizada a gravação no banco de dados e inserida uma mensagem na fila informando que um novo pedido foi criado. Ambas operações ocorrem dentro de uma mesma transação para garantirmos que as duas se completem ou nenhuma delas.

Caso ocorra falha em uma das operações dentro da transação, seja de gravação no banco de dados MySQL ou de publicação da mensagem na fila, nenhuma delas se completa e voltamos ao estado anterior.

Também em uma transação atômica, o microsserviço de Pagamento lê a mensagem da fila <b>novo_pedido</b> e grava um pagamento com status <b>Pendente</b> no MongoDB, relativo ao Id do pedido recebido na mensagem. 

Após o cliente realizar o processo de pagamento via Mercado Pago, o endpoint de webhook recebe a notificação do Mercado Pago e atualiza o status do pagamento no MongoDB. Se aprovado, o status do pagamento é atualizado para <b>Aprovado</b> no MongoDB e uma mensagem é publicada na fila <b>pagamento_aprovado</b> para que o microsserviço de Pedido dê andamento ao processo atualizando o status do pedido para <b>Em preparação</b>.

![image](https://github.com/user-attachments/assets/9140c0cc-5d83-4e0e-97f6-8c6f1d1c5462)


Abaixo, temos o trecho de código no qual atualizamos o status do pagamento no MongoDB e inserimos uma mensagem na fila <b>pagamento_aprovado</b> em uma transação:

![image](https://github.com/user-attachments/assets/4387b185-440f-419b-b87b-d66fb3ab3fac)

Caso ocorra erro no processo de pagamento, por exemplo, por uma indisponibilidade no MongoDB que impossibilite a gravação do pagamento relativo ao pedido recebido na mensagem, o microsserviço de Pagamento recoloca a mensagem na fila <b>novo_pedido</b> para ser lida novamente pela quantidade de vezes definida na variável de ambiente <b>QTDE_RETRY_PAGAMENTO</b>. 

Se as tentativas excederem o número definido, a mensagem será inserida na fila <b>pagamento_erro</b> para que o microsserviço <b>Pedido</b> altere status do pedido que consta na mensagem para <b>Cancelado</b>.

Caso o pagamento seja rejeitado pelo Mercado Pago, o cliente receberá a notificação no aplicativo e poderá alterar a forma de pagamento. Enquanto o pagamento não for aprovado, o pedido continuará com status <b>Recebido</b> e o pagamento com status <b>Pendente</b>.

## Escaneamento de vulnerabilidades - OWASP ZAP

Na fase 5, realizamos o escaneamento utilizando a ferramenta OWASP ZAP para identificarmos e tratarmos possíveis vulnerabilidades.

![image](https://github.com/user-attachments/assets/122340a0-8731-4cd9-b2b6-ed7d12681444)

![image](https://github.com/user-attachments/assets/16c4a058-d01c-43a1-be6e-2ecc42a82743)

Não foram encontradas vulnerabilidades médias ou altas, portanto, não foi necessário o tratamento.

Os relatórios OWASP-ZAP-Geração do Pagamento.pdf e OWASP-ZAP-Confirmação do Pagamento.pdf encontram-se na raíz desse projeto.

## Arquitetura
Na fase 5, adicionamos o RabbitMQ como broker de mensageria para implementarmos o padrão SAGA. 

Para rodar a aplicação, provisionamos toda a infraestrutura utilizando Terraform. Os links abaixo correspondem aos repositórios dos elementos de infraestrutura:

https://github.com/eraldoads/TechChallenge-Grupo13-K8sTerraform

https://github.com/eraldoads/TechChallenge-Grupo13-BDTerraform

https://github.com/eraldoads/TechChallenge-Grupo13-BDTerraformMongo

https://github.com/eraldoads/TechChallenge-Grupo13-RabbitMQ

Quando disparamos a Github Action, é realizado o build da aplicação e o push para o repositório criado previamente no Elastic Container Registry (ECS).
Ao final da action, é atualizada a Service no Elastic Container Service (ECS), executando assim a service que irá realizar a criação do container.

![image](https://github.com/user-attachments/assets/1925e3d6-4753-4611-99a6-647550d6497e)


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
 
