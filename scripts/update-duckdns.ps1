$domain = $env:DUCKDNS_DOMAIN
$token = $env:DUCKDNS_PRIVATE_TOKEN

# Kiểm tra nếu chưa set biến môi trường thì báo lỗi
if ([string]::IsNullOrEmpty($domain) -or [string]::IsNullOrEmpty($token)) {
    Write-Error "Chưa cấu hình biến môi trường DUCKDNS_DOMAIN hoặc DUCKDNS_TOKEN!"
    exit 1
}

Invoke-RestMethod -Uri "https://www.duckdns.org/update?domains=$domain&token=$token&ip="