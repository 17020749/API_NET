# API_NET
cài .net sdk 7.0
# Build 
dotnet build
trong file appsetting đổi server thành server trong sqlserver
 "ConnectionStrings": {
    "DefaultConString": "Server=DESKTOP-NBAPOMJ;Database=BikeStores;Trusted_Connection=True;TrustServerCertificate=True;"
  }
# Run
dotnet run
# tutorial
chạy localhost:{port}/Swagger để xem api đã build
dùng postman để test API hoặc excuted trên chạy localhost:{port}/Swagger