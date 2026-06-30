# WPF Exchange Rate Manager

실시간 환율 정보를 조회하고 MSSQL에 저장하는 WPF 데스크톱 애플리케이션입니다.

## 기술 스택

- C#, WPF, .NET Framework 4.7.2
- MSSQL (SQL Server Express)
- Newtonsoft.Json

## 주요 기능

- 실시간 환율 조회 (open.er-api.com API 사용)
- KRW / JPY / EUR 환율 MSSQL DB 저장
- 저장된 환율 기반 달러(USD) 환산 계산
- 환율 내역 조회

## 실행 방법

1. SQL Server Express 설치 후 `WB43` 데이터베이스 생성
2. `MainWindow.xaml.cs` 에서 연결 문자열의 `YOUR_SERVER_NAME` 을 본인 PC의 서버 이름으로 변경

```csharp
string con = "Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=WB43;Trusted_Connection=True;TrustServerCertificate=True";
```

3. 아래 테이블 생성

```sql
CREATE TABLE ExchangeRates (
    Id INT PRIMARY KEY IDENTITY,
    QueryTime DATETIME,
    Currency NVARCHAR(10),
    Rate FLOAT
)
```
