# Datadog Repro

Minimal .NET Lambda + CDK setup to reproduce Datadog tracing issues.

## Prerequisites

- .NET 10 SDK
- Node.js 24+
- AWS CLI configured with appropriate credentials
- CDK bootstrapped in your target account/region (`npx cdk bootstrap`)

## Project Structure

```
repo/
  src/Api/          # .NET minimal API hosted on Lambda
  cdk/              # CDK deployment project
  publish/          # (generated) published .NET artifacts
```

## Publish the .NET project

```bash
dotnet publish -c Release -o ./publish/Api ./src/Api
```

## Deploy to AWS

```bash
cd cdk
npm install
DD_API_KEY=<your-datadog-api-key> npx cdk deploy
```

The deploy output will print the Lambda Function URL. Hit `/` to test:

```bash
curl https://<function-url>/
```

## Verify

Should be seeing the `tenant.id` tag on the aspnet span

## Tear down

```bash
cd cdk
npx cdk destroy
```
