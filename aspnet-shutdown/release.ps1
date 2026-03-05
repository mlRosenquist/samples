docker-compose build --push;

Start-Job -WorkingDirectory $PWD -ScriptBlock { k6 run "k6/fast.js" --duration 10s --rps 2 --log-output=loki=http://localhost:31001/loki/api/v1/push,label.app=k6 --address localhost:6565 --out json="k6/results/fast.json" > "k6/results/slow.txt" }
Start-Job -WorkingDirectory $PWD -ScriptBlock { k6 run "k6/slow.js" --duration 10s --rps 1 --log-output=loki=http://localhost:31001/loki/api/v1/push,label.app=k6 --address localhost:6566 --out json="k6/results/fast.json" > "k6/results/fast.txt" } 
Start-Sleep 1;

kubectl rollout restart deployment ans
#kubectl apply -f ".\src\AspNetShutDown\k8s\k8s-service.yaml";

