# ============================================
# FluxPay - Makefile Oficial
# ============================================

SOLUTION=FluxPay.sln
API=src/Pay.Api/Pay.Api.csproj
WORKER_OUTBOX=src/Pay.OutboxWorker/Pay.OutboxWorker.csproj
WORKER_PROCESSOR=src/Pay.PaymentProcessorWorker/Pay.PaymentProcessorWorker.csproj
INFRA=src/Pay.Infrastructure/Pay.Infrastructure.csproj

# ============================================
# Build geral
# ============================================

build:
	dotnet build $(SOLUTION)

clean:
	dotnet clean $(SOLUTION)

# ============================================
# Run (execução padrão)
# ============================================

run-api:
	dotnet run --project $(API)

run-worker:
	dotnet run --project $(WORKER_OUTBOX)

run-processor:
	dotnet run --project $(WORKER_PROCESSOR)

# ============================================
# Watch (hot reload)
# ============================================

watch-api:
	dotnet watch run --project $(API)

watch-worker:
	dotnet watch run --project $(WORKER_OUTBOX)

watch-processor:
	dotnet watch run --project $(WORKER_PROCESSOR)

# ============================================
# Run ALL (em 3 terminais diferentes)
# ============================================

up-all:
	@echo "Execute os comandos abaixo em 3 terminais:"
	@echo "make run-api"
	@echo "make run-worker"
	@echo "make run-processor"

# ============================================
# Entity Framework
# ============================================

migration-add:
	dotnet ef migrations add $(name) -p $(INFRA) -s $(API)

migration-update:
	dotnet ef database update -p $(INFRA) -s $(API)

# ============================================
# Docker
# ============================================

docker-up:
	docker compose up -d

docker-down:
	docker compose down

docker-logs:
	docker compose logs -f

docker-restart:
	docker compose down
	docker compose up -d

# ============================================
# Ações de utilidade
# ============================================

kill-all:
	@echo "Finalizando processos .NET..."
	pkill -f Pay.Api || true
	pkill -f Pay.OutboxWorker || true
	pkill -f Pay.PaymentProcessorWorker || true
	@echo "Finalizado."

help:
	@echo "Comandos disponíveis:"
	@echo " build              - compila tudo"
	@echo " run-api            - roda API"
	@echo " run-worker         - roda OutboxWorker"
	@echo " run-processor      - roda PaymentProcessorWorker"
	@echo " watch-api          - roda API com hot reload"
	@echo " watch-worker       - roda OutboxWorker com hot reload"
	@echo " watch-processor    - roda Processor com hot reload"
	@echo " migration-add name=NomeDaMigration"
	@echo " migration-update   - aplica migrations"
	@echo " docker-up          - sobe Postgres/Kafka/Zookeeper"
	@echo " docker-down        - derruba containers"
	@echo " docker-logs        - logs do docker"
	@echo " kill-all           - mata todos .NET"
	@echo " up-all             - instrução para rodar tudo"
