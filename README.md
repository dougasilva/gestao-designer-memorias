# Gestão Designer de Memórias

Sistema para **automatizar a captação de clientes via WhatsApp**, organizar pedidos de identidade visual/digital e gerenciar **briefings e pagamentos** para um negócio de design de eventos.

Projeto pensado como **MVP pragmático**, simples de operar e fácil de evoluir.

---

## 🎯 Objetivo

Centralizar em um único sistema:
- Contatos vindos do WhatsApp
- Status do pedido (prospecção → entrega)
- Briefings dinâmicos por tipo de evento
- Controle de pagamentos (sinal + final)

Sem planilhas. Sem perda de informação. Sem retrabalho.

---

## 🏗️ Arquitetura

| Camada | Tecnologia |
|------|-----------|
| Backend | ASP.NET Core 9 (Web API) |
| Banco de Dados | SQLite |
| Frontend (Admin) | Streamlit (Python) |
| Integração WhatsApp | Webhook (n8n ou similar) |
| Hospedagem | Render / MonsterASP (API) + Streamlit Cloud |

---

## 🧠 Conceitos-chave do Domínio

### Cliente
- Identificado principalmente pelo **telefone**
- Pode existir sem nome no primeiro contato

### Pedido
- Sempre criado a partir de um contato
- Possui status bem definidos (máquina de estados simples)

### Briefing
- Flexível
- Perguntas variam conforme o tipo de evento
- Armazenado como pares Pergunta / Resposta

### Pagamento
- Regra fixa:
  - **50% na aprovação do orçamento**
  - **50% na entrega**
- Preparado para evoluir no futuro sem quebrar o modelo

---

## 🔄 Fluxo Resumido

1. Cliente envia mensagem no WhatsApp
2. Webhook recebe e registra o contato
3. Sistema cria Cliente + Pedido em *Prospecção*
4. Pedido evolui conforme interação:
   - Orçamento
   - Briefing
   - Pagamento
   - Entrega
5. Tudo visível no dashboard administrativo

---

## 📊 Status do Projeto

🚧 **Em desenvolvimento (MVP)**

Backlog e progresso são gerenciados via **GitHub Projects (v2)** com Kanban.

---

## 🚀 Como rodar localmente (Backend)

Pré-requisitos:
- .NET SDK 9
- Git

```bash
git clone https://github.com/<seu-usuario>/gestao-designer-memorias.git
cd gestao-designer-memorias
dotnet restore
dotnet run

