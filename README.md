# KeywordChart
## 프로젝트 목표
 * OpenAPI 활용하여 연관검색어 검색 기능을 구현한다.
 * Chart Library를 통해 검색어 결과를 효과적으로 시각화한다. 

## 기술명세
#### FrontEnd
  ![HTML5-E34F26](https://user-images.githubusercontent.com/58022014/172148937-578d0786-0778-4db2-81db-568b724a192b.svg)(cshtml) ![CSS3-1572B6](https://user-images.githubusercontent.com/58022014/172148850-e522015b-d8e9-4345-8583-0125c15938dd.svg) ![JavaScript-F7DF1E](https://user-images.githubusercontent.com/58022014/172148954-06609d0a-5d96-4519-89e2-d70f9abed91b.svg)
#### BackEnd
  ![NET Core-512BD4](https://user-images.githubusercontent.com/58022014/172149484-7c18b217-6b49-4f3f-940a-4853ae26f462.svg)

####  DataBase
  ![MySQL-4479A1](https://user-images.githubusercontent.com/58022014/172148993-3a9cfa64-65fd-499c-ae5f-783ab58e029c.svg)

#### UI 
  * ASP.NET Core MVC
#### 라이브러리
  [<img src="[https://github.com/favicon.ico](https://user-images.githubusercontent.com/58022014/172142754-a335b3e3-f87f-459c-93ba-4c1c3f47b1da.svg)" width="150" height="30">](https://www.highcharts.com/)
#### 외부API
  [![Naver광고API-03C75A](https://user-images.githubusercontent.com/58022014/172149017-1007bf49-7d58-4cc5-9788-0e785f94aed5.svg)](https://manage.searchad.naver.com/)

## 기능명세
1. 기본 환경 셋팅
  * .NET Core 셋팅
  * VS 2022 셋팅
  * MySql 셋팅
  * git 및 github 연결
2. 화면 설계
  * 검색+차트, 검색어 이력 등 약 2장
  * PPT 파일로 작성
3. 기능 구현
  1. 연관검색어 검색 
    * API 신청
    * Json 파싱
    * 검색 결과 리스트
  2. 검색어 이력
    * 검색어 이력 저장
  3. 검색 결과 차트
    * 연관검색어 비중(pie)
    * PC/모바일 연관검색어 건수(stacked column)
<img width="516" alt="차트" src="https://user-images.githubusercontent.com/58022014/172150909-952f67f3-69b4-4a5b-b393-e3817f045f24.png">
