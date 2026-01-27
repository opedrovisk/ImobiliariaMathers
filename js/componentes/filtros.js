const filtros = {
  tipo: [],
  garagem: [],
  dormitorios: [],
  espaco: []
};

function configurarFiltros() {
  const btnFiltrar = document.getElementById("btn-filtrar");
  if (btnFiltrar) {
    btnFiltrar.addEventListener("click", function(event) {
      event.preventDefault();
      filtros.tipo = [];
      filtros.garagem = [];
      filtros.dormitorios = [];
      filtros.espaco = [];

      if (document.getElementById("tipo-casa").checked) {
        filtros.tipo.push("casa");
      }
      if (document.getElementById("tipo-apartamento").checked) {
        filtros.tipo.push("apartamento");
      }

      if (document.getElementById("garagem-sim").checked) {
        filtros.garagem.push(true);
      }
      if (document.getElementById("garagem-nao").checked) {
        filtros.garagem.push(false);
      }

      ["dorm-1", "dorm-2", "dorm-3", "dorm-4"].forEach(id => {
        const checkbox = document.getElementById(id);
        if (checkbox.checked) {
          filtros.dormitorios.push(checkbox.value);
        }
      });

      ["espaco-0-50", "espaco-51-100", "espaco-101-150", "espaco-150"].forEach(id => {
        const checkbox = document.getElementById(id);
        if (checkbox.checked) {
          filtros.espaco.push(checkbox.value);
        }
      });

      aplicarFiltros();
    });
  }

  const btnLimpar = document.getElementById("btn-limpar");
  if (btnLimpar) {
    btnLimpar.addEventListener("click", function(event) {
      event.preventDefault();

      document.querySelectorAll("#filtros input[type='checkbox']").forEach(cb => cb.checked = false);

      filtros.tipo = [];
      filtros.garagem = [];
      filtros.dormitorios = [];
      filtros.espaco = [];

      aplicarFiltros();
    });
  }
}

function aplicarFiltros() {
  const filtrados = imoveis.filter(imovel => {
    let passa = true;

    if (filtros.tipo.length > 0 && !filtros.tipo.includes(imovel.tipo)) {
      passa = false;
    }

    if (filtros.garagem.length > 0 && !filtros.garagem.includes(imovel.garagem)) {
      passa = false;
    }

    if (filtros.dormitorios.length > 0 && !filtros.dormitorios.includes(String(imovel.dormitorios))) {
      passa = false;
    }

    if (filtros.espaco.length > 0) {
      const passouEspaco = filtros.espaco.some(faixa => {
        if (faixa === "150+") {
          return imovel.espaco >= 150;
        } else {
          const [min, max] = faixa.split("-").map(Number);
          return imovel.espaco >= min && imovel.espaco <= max;
        }
      });

      if (!passouEspaco) {
        passa = false;
      }
    }

    return passa;
  });

  gerarCards(filtrados);
  document.getElementById("resultado-contagem").textContent = `Mostrando ${filtrados.length} imóv${filtrados.length !== 1 ? 'eis' : 'el'}`;
}

document.addEventListener("DOMContentLoaded", function() {
  configurarFiltros();
  carregarImoveis();
});
