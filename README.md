# Expense Tracker

Repository for the Expense Tracker that contains both the API and the Frontend. REST API built with .NET which allows you to create, delete, update and read multiple users accounts,wallets for budget organisation,transactions and standard expenses for reccuring expenses in order to have better money managment. And a clean simple frontend  build in React for easy usage.

## How to start

You can either run the app locally or use Docker, I suggest the second option because it is much easier to run with everything set up:

1. Navigate to compose folder from root directory

```
cd docker/compose
```

2. Run the following command for a new build

```
docker compose up --build
```

3. Both the app and the PostgreSql Database should be running, access swagger UI on localhost:8080

4. For a new clean build you can run the next command, and rerun the step 2.

```
docker compose down -v
```

For frontend follow the next steps:

1. Navigate to expense-tracker folder from root directory

```
cd Frontend/expense-tracker
```

2. Install all the packages

```
npm install
```

3. Run the frontend

```
npm run dev
```

4. The app should be running on localhost:5173

## How to use

Now you can either create your own account or use the following account aready in the initial database

| Email | Password |
|-------|----------|
|mujo.mujic@gmail.com | Password123!|
|imran.vlajcic@gmail.com | Password123!|
|demo.user@gmail.com | Password123!|

From there staring making your own wallets and adding new transactions to them, you can see your overall stats on the dashboard or on the statistic page, if you need to remove them it will automaticly wif you wallet balance, for any transaction that might happen in certain frequency you can create standard expenses. The background service of the app will run at midnight and check if there are any transactions to be created. For better filtering use the expense page with multiple options such as amount range, cash or card, income or expense, category etc.  


