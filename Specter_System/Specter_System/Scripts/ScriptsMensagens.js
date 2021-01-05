
function RealizarCadastroUsuario(mensagem) {

    if (mensagem = "Email já encontra-se cadastrado") {
        alert('E-mail já encontra-se cadastrado');
    }
    else if (mensagem = "Senhas não conferem") {
        alert('Senhas não conferem');
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