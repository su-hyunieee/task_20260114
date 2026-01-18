직원(Employee) 정보를 CSV/JSON으로 입력받아 SQLite에 저장/ 조회 하는 프로젝트

- .NET: net 9.0.309
- DB: SQLite (employee.db)
- ORM: EF Core
- 테스트 도구: Postman

# 1. 프로젝트 구조

- CoreServer.Api : Controller, Program.cs
- CoreServer.Application : 서비스
- CoreServer.Domain : 도메인 모델
- CoreServer.Infrastructure : EF Core, Repository, Db

# 2. 실행 방법

## 2.1 요구사항

- .NET SDK 9 설치
- SQLite 설치  
  `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
- OpenApi 설치  
  `dotnet add package Microsoft.AspNetCore.OpenApi`

## 2.2 실행

- `git clone https://github.com/su-hyunieee/task_20260114.git`
- `dotnet restore`
- `dotnet run --project CoreServer.Api`

## 2.3 실행 확인

- git clone 이후 build 성공 ( *** CoreServer Start **** )
- API 실행 후 Postman을 통해 결과 확인 가능

# 3. 데이터베이스 설정

- DB: SQLite
- 파일명: employee.db
- 위치: CoreServer.Api 프로젝트 기준 자동 생성
- 애플리케이션 실행 시 테이블이 자동 생성됩니다.

# 4. 데이터 입력 방식

직원 데이터는 CSV / JSON 형식으로 입력할 수 있습니다.

- CSV 텍스트 입력 (Content-Type: text/plain)
- JSON 텍스트 입력 (Content-Type: application/json)
- CSV / JSON 파일 업로드 (multipart/form-data)

# 5. API Endpoints

- GET /api/employee?page={page}&pageSize={pageSize}  
  직원 목록 조회 (페이징)

- GET /api/employee/{name}  
  이름으로 직원 조회

- POST /api/employee?format=csv | json  
  CSV / JSON 텍스트 입력으로 직원 데이터 저장

- POST /api/employee/upload?format=csv | json  
  CSV / JSON 파일 업로드로 직원 데이터 저장

# 6. 테스트 방법

- Postman을 사용하여 API 테스트
- GET / POST / 파일 업로드 요청 모두 Postman에서 확인 가능
