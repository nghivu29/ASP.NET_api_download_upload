# ASP.NET_api_download_upload

B1: Sử dụng một dịch vụ cho thuê VPS bất kì. Ví dụ này là Vultr.

B2: Cài đặt môi trường.
sudo rpm — import https://packages.microsoft.com/keys/microsoft.asc
sudo sh -c ‘echo -e “[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl= https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc" > /etc/yum.repos.d/dotnetdev.repo’
sudo yum update -y
sudo yum install libunwind libicu -y
sudo yum install dotnet-sdk-6.0
sudo yum install aspnetcore-runtime-6.0

B3: Cấu hình NGINX
sudo yum install nginx -y
File MyApi.conf:
server {
  listen 80;
  location /weatherforecast {
        proxy_pass http://localhost:5000/weatherforecast;
  }
  location /api/TodoItems {
        proxy_pass http://localhost:5000/api/TodoItems;
  }
  location /UploadFile {
        proxy_pass http://localhost:5000/UploadFile;
  }
}

B4:  Tạo service
File MyApi.service:
[Unit]
Description=My Api Service
[Service]
WorkingDirectory=/home/www/MyApi
ExecStart=/usr/bin/dotnet /home/www/MyApi/MyApi.dll
Restart=always
RestartSec=20
SyslogIdentifier=dotnet-myapi
User=nginx
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000
[Install]
WantedBy=multi-user.target

B5: Public ASP.NET project và Sử dụng FTP để chuyển source lên VPS
 

B6: cấp quyền truy cập project cho NGINX
sudo chown nginx:nginx -R /home/www/MyApi

B7: Khởi động dịch vụ
sudo systemctl start MyApi
sudo systemctl enable MyApi.service
sudo systemctl start nginx
sudo systemctl reload nginx

B8: Mở cổng kết nối 80 và 443 trên tường lửa
sudo firewall-cmd — zone=public — permanent — add-service=http
sudo firewall-cmd — zone=public — permanent — add-service=https
sudo firewall-cmd — reload
sudo systemctl enable firewalld.service
