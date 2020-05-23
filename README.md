# Desafio Easynvest
Desafio da Easynvest para criação de API de Investimentos.

## Premissas de regras negócios
* Para cálculo de resgate, é levado  em consideração taxa de custódia (porcentagem de perda sob o valor investido), IR e IOF. Desta forma, `valor do resgate = valor total - taxa custódia - IR - IOF`. Para taxa de custódia, é considerado a porcentagem de perda descrita no desafio sob o valor investido.
* Um investimento de 6 meses, por exemplo, pode estar ao mesmo tempo com mais da metade do tempo em custódia, e estar até 3 meses antes do vencimento. Neste caso, é considerado 15% para taxa de custódia.
* A data de resgate considerada para cálculo de perda é a data de chamada da API (data atual).
* A taxa de custódia é aplicável somente para resgates anteriores ao vencimento. Para resgates cuja data de vencimento < data de resgate, a taxa de custódia = 0
* Nos investimentos com rentabilidade menor ou igual a zero, não há indidência de IR. Para os demais, a taxa IR corresponde à porcentagem descrita no desafio.


## Premissas técnicas
* No contrato de LCIs, o valor investido corresponde ao campo `capitalInvestido` do JSON, o campo valor total corresponde ao campo `capitalAtual` do JSON.
* No contrato de Fundos, o valor investido corresponde ao campo `capitalInvestido` do JSON, o campo valor total corresponde ao campo `ValorAtual` do JSON, a data de vencimento corresponde ao campo `dataResgate` do JSON, a data de operação corresponde ao campo `dataCompra` do JSON.
* No contrato de Tesouro Direto, a data de operação corresponde ao campo `dataDeCompra` do JSON.
* Para os campos que não foram comentados acima, os campos do contrato de entrada correspondem exatamente aos valores retornados no JSON. 
* A api  `GET api/v1/investimentos` foi desenvolvida para atender a API proposta no desafio. Nela é possível passar o identificador do cliente, através do queryParameter `idCliente`. Como não faz parte dos requisitos iniciais dessa API, este parâmetro é opcional.
* Os endpoints de cada tipo de investimento podem ser configurados no arquivo de configuração do projeto de API: `Easynvest.Desafio.Investimentos.Api\appsettings.json`.



