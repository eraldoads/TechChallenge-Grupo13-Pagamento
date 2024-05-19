Funcionalidade: Gerenciar Pagamentos

Como usuário do sistema 
Eu quero buscar um Pagamento pelo ID do Pedido
E receber o seu Status

@tag1
Cenario: [Buscar Status do Pagamento pelo ID do Pedido]
	Dado que o Pedido com ID 1 está com o pagamento Pendente
	Quando requisitar a busca do Status do Pagamento por seu ID
	Então o Status do Pagamento é exibido com sucesso