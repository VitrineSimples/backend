# 🌟 Guren API

**Guren** é uma API RESTful robusta e escalável, desenvolvida em **ASP.NET Core**, projetada para gerenciar **pedidos**, **lojas**, **produtos**, **clientes** e **campanhas sazonais** de forma eficiente e intuitiva.

---

## ✨ Funcionalidades

- 🏬 **Gerenciamento de Lojas**: Crie, edite, liste e remova lojas com facilidade.
- 📦 **Gerenciamento de Produtos**: Administre o catálogo de produtos, incluindo categorias e estoque.
- 👥 **Gerenciamento de Clientes**: Cadastre e gerencie informações de clientes.
- 🛒 **Gerenciamento de Pedidos**: Controle pedidos, incluindo status e histórico.
- 🎉 **Gerenciamento de Campanhas Sazonais**: Crie e gerencie campanhas promocionais para produtos.

---

## 🗂 Estrutura do Projeto

O projeto é organizado em uma arquitetura limpa e modular:

- 📁 **Guren/Model**: Contém as entidades do sistema, representando a estrutura de dados.
- 📁 **Guren/DTO**: Objetos de Transferência de Dados (DTOs) para comunicação eficiente entre camadas.
- 📁 **Guren/Controllers**: Controladores da API, responsáveis pelas rotas e lógica de negócio.
- 📁 **Guren/Database**: Configurações do **Entity Framework Core**, incluindo o `DbContext` e migrations.

---

## 🚀 Como Executar o Projeto

Siga os passos abaixo para rodar a API localmente:

### 1. Clonar o Repositório
```bash
git clone https://github.com/seu-usuario/guren-api.git
cd guren-api
```

### 2. Configurar o Banco de Dados
```bash
dotnet ef database update
```

### 3. Rodar
```bash
dotnet run
```

Caso funcione, vai abrir a documentação do swagger

---

### Desenvolvido com 💖 por GURIS
