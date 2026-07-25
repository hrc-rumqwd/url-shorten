$outPort=5672
$inPort=5672
$outAdminPort=15672
$inAdminPort=15672
$version="4-management"


docker run -d -it --rm --name rabbitmq -p "${inPort}:${outPort}" -p "${inAdminPort}:${outAdminPort}" "rabbitmq:${version}"