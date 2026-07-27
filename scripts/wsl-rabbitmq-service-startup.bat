@echo off
:: Help render Vietnamese language do not cause error
chcp  65001 >nul
title Start Docker and RabbitMQ through WSL2

echo Run WSL with Docker service...
:: Chạy wsl và start Docker service trong wsl
wsl -u root service docker start

set CONTAINER_NAME=local-rabbitmq

:: Check container is existing
wsl docker ps -a --format "{{.Names}}" | findstr /R /C:"^%CONTAINER_NAME%$" >nul

if %%errorlevel% equ 0 (
	echo [INFO] Container '%CONTAINER_NAME%' existed, restart again....
	wsl docker start %CONTAINER_NAME%
) else (
	echo [INFO] Create new container and run '%CONTAINER_NAME%'
	wsl docker run -d --name %CONTAINER_NAME% -p 5672:5672 -p 15672:15672 rabbitmq:4-management
)


echo Run RabbitMQ successfully!

pause
