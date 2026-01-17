직원(Employee) 정보를 CSV/JSON으로 입력받아 SQLite에 저장/ 조회 하는 프로젝트

- .NET: net9.0
- DB: SQLite (employee.db)
- ORM: EF Core
- 테스트 도구: Postman

1. 프로젝트 구조
- CoreServer.Api : Controller, Program.cs
- CoreServer.Application : 서비스 
- CoreServer.Domain : 도메인 모델
- CoreServer.Infrastructure : EF Core, Repository, Db

2. 실행 방법
 2.1 요구사항
- .NET SDK 9 설치
 2.2 실행
dotnet restore
dotnet run --project CoreServer.Api
