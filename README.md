Projeto Fluxo de Caixa e Lançamentos
====================================

Tecnologias Utilizadas
----------------------

*   O projeto foi construído utilizando **.NET 8**.
    
*   Para persistência de dados, utilizamos **MariaDB 10.5** devido ao seu desempenho superior e otimizações em operações de armazenamento e replicação.
    
*   **RabbitMQ** foi utilizado para gerenciamento da fila de processamento de relatórios, permitindo o processamento eficiente e assíncrono de grandes volumes de transações diárias.
    
*   O projeto **Lançamentos** utiliza **HostedService** do .NET para o processamento contínuo e em segundo plano de tarefas recorrentes.
    

### Justificativa para a escolha do MariaDB:

Optamos pelo **MariaDB 10.5** por causa do seu **desempenho e otimização** em várias situações críticas. Algumas das razões principais incluem:
    
*   **Cache de consultas** mais eficiente, acelerando respostas.
    
*   Melhorias em **operações de replicação** e **escalabilidade**, o que o torna ideal para aplicações que lidam com grandes volumes de dados, como o controle de fluxo de caixa.
    

Desenho da Solução / Arquitetura
--------------------------------

A solução foi desenhada com foco em **escalabilidade** e **manutenibilidade**, adotando o padrão de **Clean Architecture**. Isso facilita a implementação de novas funcionalidades e a manutenção do sistema ao longo do tempo, separando bem as responsabilidades entre camadas de domínio, aplicação e infraestrutura.

### Conceitos e Bibliotecas Utilizadas:

#### Projeto **Fluxo de Caixa**:

*   **BCrypt.Net-Next**: Para hash de senhas e segurança de usuários.
    
*   **Microsoft.AspNetCore.Authentication.JwtBearer**: Autenticação baseada em tokens JWT para segurança.
    
*   **Microsoft.EntityFrameworkCore**: Interação com o banco de dados MariaDB.
    
*   **Microsoft.EntityFrameworkCore.Tools**: Ferramentas para migrações e gerenciamento de banco de dados.
    
*   **Pomelo.EntityFrameworkCore.MySql**: Driver para MariaDB.
    
*   **RabbitMQ.Client**: Para comunicação com o servidor RabbitMQ e processamento de filas.
    
*   **Swashbuckle.AspNetCore**: Geração automática de documentação da API usando Swagger.
    

#### Projeto **Lançamentos**:

*   **BCrypt.Net-Next**: Para segurança de senhas.
    
*   **Microsoft.AspNetCore.Authentication.JwtBearer**: Autenticação baseada em JWT.
    
*   **Microsoft.EntityFrameworkCore**: Para interação com MariaDB.
    
*   **Microsoft.EntityFrameworkCore.Tools**: Ferramentas de migração.
    
*   **Moq**: Para criação de mocks em testes unitários.
    
*   **Pomelo.EntityFrameworkCore.MySql**: Driver específico para MariaDB.
    
*   **RabbitMQ.Client**: Integração com RabbitMQ para processamento de filas.
    
*   **Swashbuckle.AspNetCore**: Geração de documentação da API com Swagger.
    
*   **xUnit**: Para testes unitários.
    

Executando o Projeto
--------------------

### Pré-requisitos:

*   Docker instalado na máquina para facilitar a execução e configuração dos serviços.
    
*   Coleção do Postman para testes de APIs (disponível na pasta /PostmanCollection).
    

### Passos para Configurar e Executar o Projeto:

#### 1\. Subir o RabbitMQ no Docker:

O RabbitMQ será responsável pelo gerenciamento de filas de mensagens. Para subir o RabbitMQ no Docker, execute o seguinte comando:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopiar códigodocker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management   `

Esse comando irá:

*   Subir o RabbitMQ na porta **5672** para conexão com a aplicação.
    
*   Subir o **RabbitMQ Management Console** na porta **15672** para monitorar o status das filas e mensagens. Acesse via http://localhost:15672 com usuário **guest** e senha **guest**.
    

#### 2\. Subir o Banco de Dados MariaDB no Docker:

O MariaDB 10.5 será usado para persistir os dados transacionais da aplicação. Para rodá-lo, execute o comando abaixo:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopiar códigodocker run --name mariadb -e MYSQL_ROOT_PASSWORD=alolopes -e MYSQL_DATABASE=fluxodecaixa -e MYSQL_USER=user -e MYSQL_PASSWORD=alolopes -p 3306:3306 -d mariadb:10.5   `

Esse comando irá:

*   Criar um banco de dados chamado **fluxodecaixa**.
    
*   Criar um usuário **user** com a senha **alolopes**.
    

#### 3\. Rodar as Migrações no Projeto Fluxo de Caixa:

Antes de executar a aplicação **Fluxo de Caixa**, é necessário aplicar as migrações para configurar o banco de dados. Siga os seguintes passos:

1.  Navegue até a pasta do projeto **FluxodeCaixa**.
    
2.  Execute o comando para aplicar as migrações:
    

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopiar códigodotnet ef database update   `

Isso irá rodar as migrações necessárias e preparar o banco de dados para a aplicação.

#### 4\. Executar a Aplicação:

Após configurar RabbitMQ, MariaDB 10.5 e rodar as migrações, você pode rodar o projeto com o seguinte comando:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopiar códigodotnet run   `

#### 5\. Executar via Docker-Compose (Opcional):

Para simplificar o processo, você pode utilizar o **Docker-Compose** para subir o RabbitMQ, MariaDB, e a aplicação de uma só vez.

Crie um arquivo docker-compose.yml com o seguinte conteúdo:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   yamlCopiar códigoversion: '3.8'  services:    mariadb:      image: mariadb:10.5      environment:        MYSQL_ROOT_PASSWORD: alolopes        MYSQL_DATABASE=fluxodecaixa        MYSQL_USER=user        MYSQL_PASSWORD=alolopes      ports:        - "3306:3306"    rabbitmq:      image: rabbitmq:3-management      ports:        - "5672:5672"        - "15672:15672"    app:      build: .      depends_on:        - mariadb        - rabbitmq      ports:        - "5000:5000"        - "5001:5001"      environment:        - ConnectionStrings__DefaultConnection=Server=mariadb;Database=fluxodecaixa;User=user;Password=alolopes;        - RabbitMQ__HostName=rabbitmq   `

Depois de configurar o arquivo, execute:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopiar códigodocker-compose up   `

Esse comando irá levantar todos os serviços e a aplicação ao mesmo tempo.

Autenticação JWT
----------------

### Obtendo o Token JWT

Para fazer chamadas aos serviços protegidos pela API, você precisará obter um token JWT (JSON Web Token) para autenticação. Siga os passos abaixo para obter o token e incluí-lo nas requisições.

#### 1\. Obter o Token:

Envie uma requisição POST ao endpoint de login da API fornecendo as credenciais de usuário (e.g., nome de usuário = alolopes e senha=12345).

##### Exemplo de Requisição:

**Endpoint:**

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   httpCopiar códigoPOST /api/auth/login   `

**Corpo da Requisição (JSON):**

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   jsonCopiar código{    "username": "yourusername",    "password": "yourpassword"  }   `

**Resposta Esperada (JSON):**

Se as credenciais forem válidas, a API retornará um token JWT no corpo da resposta:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   jsonCopiar código{    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."  }   `

Guarde esse token para incluir nas próximas requisições.

#### 2\. Incluir o Token JWT nas Chamadas:

Para chamar os serviços protegidos pela API, inclua o token JWT no cabeçalho Authorization de todas as requisições, seguindo o esquema Bearer.

##### Exemplo de Requisição com Token:

**Endpoint:**

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   httpCopiar códigoGET /api/lancamentos   `

**Cabeçalho:**

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   httpCopiar códigoAuthorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...   `

Esse token autentica o usuário e permite o acesso aos endpoints protegidos da API.

### Testando com Postman:

1.  **Obter o Token**:
    
    *   No **Postman**, faça uma requisição POST para o endpoint /api/auth/login com as credenciais.
        
    *   Copie o token JWT da resposta.
        
2.  Agora, todas as chamadas que você fizer no Postman serão autenticadas com o token JWT.
    
    *   Para qualquer outra requisição à API, vá até a aba "Authorization" no Postman.
        
    *   Selecione o tipo **Bearer Token**.
        
    *   Cole o token JWT obtido anteriormente no campo de texto.
        

Casos de Uso da Aplicação
-------------------------

### Controle de Lançamentos e Serviço do Consolidado Diário:

*   A aplicação atende aos requisitos de negócio, com serviços dedicados para o **controle de lançamentos** e o **consolidado diário**. Isso garante que as transações financeiras sejam processadas corretamente e de maneira eficiente ao longo do dia.
    

### Estrutura do Projeto:

*   Utilização de **Clean Architecture**, que facilita a manutenção e testabilidade do código. Também são adotados conceitos como **Ports and Adapters** e boas práticas como **SOLID** e **DRY**.
    
