@echo off
echo ==============================================
echo Iniciando os projetos do Supermercado...
echo ==============================================

echo [1/2] Iniciando Supermercado.Api (Backend)
start "Supermercado.Api" cmd /c "title API - Backend && dotnet run --project Supermercado.Api\Supermercado.Api.csproj"

echo [2/2] Iniciando Supermercado.Web (Frontend)
start "Supermercado.Web" cmd /c "title Web - Frontend && dotnet run --project Supermercado.Web\Supermercado.Web.csproj"

echo.
echo Os projetos estao abrindo em novas janelas do terminal!
echo Para encerrar, basta fechar as janelas que foram abertas.
echo ==============================================
