pushd .
cd bin
start riskcontrol.com -db "Default" -u fabio.marreco -p ""
start msmqworker.exe
cd test
testadorrelatorios.exe -path "..\..\test\casostestes\Relatorios Validacao Testes Nao Validados" -files "*Casos Testes Globais - FluxoCaixa*"
set a=%ERRORLEVEL%
taskkill /im riskcontrol* /im msmqworker*
popd
exit /b %a%