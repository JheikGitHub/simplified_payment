# Desafio Back-end PicPay com .NET 8

## Objetivo:

Temos 2 tipos de usu�rios, os comuns e lojistas, ambos t�m carteira com dinheiro e realizam transfer�ncias entre eles. Vamos nos atentar **somente** ao fluxo de transfer�ncia entre dois usu�rios.

Requisitos:

-   Para ambos tipos de usu�rio, precisamos do Nome Completo, CPF, e-mail e Senha. CPF/CNPJ e e-mails devem ser �nicos no sistema. Sendo assim, seu sistema deve permitir apenas um cadastro com o mesmo CPF ou endere�o de e-mail.

-   Usu�rios podem enviar dinheiro (efetuar transfer�ncia) para lojistas e entre usu�rios.

-   Lojistas **s� recebem** transfer�ncias, n�o enviam dinheiro para ningu�m.

-   Validar se o usu�rio tem saldo antes da transfer�ncia.

-   Antes de finalizar a transfer�ncia, deve-se consultar um servi�o autorizador externo, use este mock para simular (https://run.mocky.io/v3/5794d450-d2e2-4412-8131-73d0293ac1cc).

-   A opera��o de transfer�ncia deve ser uma transa��o (ou seja, revertida em qualquer caso de inconsist�ncia) e o dinheiro deve voltar para a carteira do usu�rio que envia.

-   No recebimento de pagamento, o usu�rio ou lojista precisa receber notifica��o (envio de email, sms) enviada por um servi�o de terceiro e eventualmente este servi�o pode estar indispon�vel/inst�vel. Use este mock para simular o envio (https://run.mocky.io/v3/54dc2cf1-3add-45b5-b5a9-6bf7e7f1f4a6).

-   Este servi�o deve ser RESTFul.

### Para mais detalhes segue o link do desafio: https://github.com/PicPay/picpay-desafio-backend