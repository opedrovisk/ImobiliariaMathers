import 'bootstrap';

document.addEventListener('DOMContentLoaded', () => {
  const searchForm = document.querySelector('.search-form');

  if (searchForm) {
    searchForm.addEventListener('submit', (e) => {
      e.preventDefault();

      const tipo = document.getElementById('input-tipo').value;
      const garagem = document.getElementById('input-garagem').value;
      const dormitorios = document.getElementById('input-dormitorios').value;
      const espaco = document.getElementById('input-espaco').value;

      const params = new URLSearchParams();
      if (tipo) params.append('tipo', tipo);
      if (garagem) params.append('garagem', garagem);
      if (dormitorios) params.append('dormitorios', dormitorios);
      if (espaco) params.append('espaco', espaco);

      window.location.href = `imoveis.html?${params.toString()}`;
    });

    function criarCardImovel(imovel) {
      return `
        <div class="col-12 col-md-6 col-lg-3">
            <div class="card-imovel">
                <div class="card-imovel-imagem">
                    <img src="${imovel.imagem}" alt="${imovel.titulo}">
                    <span class="badge-tipo">${imovel.tipo === 'casa' ? 'Casa' : 'Apartamento'}</span>
                </div>
                <div class="card-imovel-conteudo">
                    <h3 class="card-imovel-titulo">${imovel.titulo}</h3>
                    <p class="card-imovel-endereco">
                        <i class="bi bi-geo-alt"></i> ${imovel.endereco}
                    </p>
                    <p class="card-imovel-descricao">${imovel.descricao}</p>
                    <div class="card-imovel-detalhes">
                        <span><i class="bi bi-door-closed"></i> ${imovel.dormitorios} dorm.</span>
                        <span><i class="bi bi-rulers"></i> ${imovel.area} m²</span>
                        <span><i class="bi bi-car-front"></i> ${imovel.garagem ? 'Com garagem' : 'Sem garagem'}</span>
                    </div>
                    <div class="card-imovel-footer">
                        <span class="card-imovel-preco">${imovel.preco}</span>
                        <button class="btn-ver-mais">Ver mais</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    }

    function exibirImoveis(imoveis) {
      const cardsContainer = document.getElementById('cards-container');
      const resultadosSection = document.getElementById('resultados');

      if (imoveis.length === 0) {
        cardsContainer.innerHTML = `
            <div class="col-12">
                <div class="sem-resultados">
                    <i class="bi bi-house-x"></i>
                    <h3>Nenhum imóvel encontrado</h3>
                    <p>Tente ajustar os filtros de busca</p>
                </div>
            </div>
        `;
      } else {
        cardsContainer.innerHTML = imoveis.map(criarCardImovel).join('');
      }

      resultadosSection.style.display = 'block';
      resultadosSection.scrollIntoView({ behavior: 'smooth' });
    }

    function filtrarImoveis() {
      const tipo = document.getElementById('input-tipo').value;
      const garagem = document.getElementById('input-garagem').value;
      const dormitorios = document.getElementById('input-dormitorios').value;
      const espaco = document.getElementById('input-espaco').value;

      let imoveisFiltrados = [...imoveisPlaceholder];

      if (tipo) {
        imoveisFiltrados = imoveisFiltrados.filter(imovel => imovel.tipo === tipo);
      }

      if (garagem) {
        const temGaragem = garagem === 'true';
        imoveisFiltrados = imoveisFiltrados.filter(imovel => imovel.garagem === temGaragem);
      }

      if (dormitorios) {
        const numDormitorios = parseInt(dormitorios);
        imoveisFiltrados = imoveisFiltrados.filter(imovel => {
          if (numDormitorios === 5) {
            return imovel.dormitorios >= 5;
          }
          return imovel.dormitorios === numDormitorios;
        });
      }

      if (espaco) {
        imoveisFiltrados = imoveisFiltrados.filter(imovel => {
          switch (espaco) {
            case '0-50':
              return imovel.area <= 50;
            case '51-100':
              return imovel.area > 50 && imovel.area <= 100;
            case '101-150':
              return imovel.area > 100 && imovel.area <= 150;
            case '150+':
              return imovel.area > 150;
            default:
              return true;
          }
        });
      }

      return imoveisFiltrados;
    }

    document.addEventListener('DOMContentLoaded', () => {
      const searchForm = document.querySelector('.search-form');

      searchForm.addEventListener('submit', (e) => {
        e.preventDefault();

        const imoveisFiltrados = filtrarImoveis();
        exibirImoveis(imoveisFiltrados);
      });
    });
