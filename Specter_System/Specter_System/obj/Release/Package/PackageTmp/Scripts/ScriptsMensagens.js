
function RealizarCadastroUsuario(mensagem) {

    if (mensagem == "Email ja encontra-se cadastrado") {
        alert('E-mail já encontra-se cadastrado');
    }
    else if (mensagem == "Senhas nao conferem") {
        alert('Senhas não conferem');
    }
    else if (mensagem == "Informe um e-mail valido") {
        alert('Informe um e-mail válido')
    }
}

function RealizarCadastroPessoa(dado) {

    if (dado == "CPF ja encontra-se cadastrado") {
        alert('CPF já encontra-se cadastrado');
    }
    else if (dado == "Cpf informado nao e valido") {
        alert('Cpf informado não á válido')
    }
    else {
        alert('Erro ao realizar Cadastro');
    }
}

function ValidarLogin(mensagem) {

    if (mensagem == 'Usuario ja encontra-se em uso') {
        alert('Usuário já encontra-se em uso');
    }

    else if (mensagem == 'Usuario ou senha invalido') {
        alert('Usuário ou senha inválido');
        document.getElementById("Email").value = '';
        document.getElementById("Senha").value = '';
    }
}

function RecuperarSenha(resp) {

    if (resp == "Email enviado com sucesso") {
        alert('Código enviado por e-mail, verifique a caixa de spam');
        document.getElementById("formDados").style.display = "none";
        document.getElementById("formAlterarSenha").style.display = "inline";
             
    }

    else if (resp == "Senhas nao conferem")
        alert('Senhas não conferem')

    else if (resp == "Senha alterada com sucesso")
        alert('Senha alterada com sucesso');

    else if (resp == "Codigo invalido")
        alert('Código fornecido inválido')

    else if (resp == "nao localizado")
        alert('Dados não localizado')
   
    
}