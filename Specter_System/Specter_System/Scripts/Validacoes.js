function ValidarCpf(CPF) {

    var cpf = CPF.value;
    var tamanho = cpf.length;

    if (tamanho < 14) {
        alert('CPF deve conter 11 DIGÍTOS');
        document.getElementById("cpf").value = "";
    }
}

function ValidaCPF(){	
	var RegraValida=document.getElementById("cpf").value; 
	//var cpfValido = /^(([0-9]{3}.[0-9]{3}.[0-9]{3}-[0-9]{2})|([0-9]{11}))$/;	 

    if (RegraValida.length < 14) {
        alert('CPF deve conter 11 DIGÍTOS')
    }
}

function fMasc(objeto,mascara) {
    obj=objeto
    masc=mascara
    setTimeout("fMascEx()",1)
}

function fMascEx() {
    obj.value=masc(obj.value)
}

function mCPF(cpf){
    cpf=cpf.replace(/\D/g,"")
    cpf=cpf.replace(/(\d{3})(\d)/,"$1.$2")
    cpf=cpf.replace(/(\d{3})(\d)/,"$1.$2")
    cpf=cpf.replace(/(\d{3})(\d{1,2})$/,"$1-$2")
return cpf
}
