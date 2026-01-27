const imoveisPlaceholder = [
    {
        id: 1,
        titulo: "Apartamento Moderno no Centro",
        tipo: "apartamento",
        dormitorios: 3,
        garagem: true,
        area: 85,
        preco: "R$ 450.000",
        endereco: "Rua das Flores, 123 - Centro",
        imagem: "assets/placeholder01.jpg",
        descricao: "Apartamento completamente reformado com acabamento de primeira linha"
    },
    {
        id: 2,
        titulo: "Casa Espaçosa com Jardim",
        tipo: "casa",
        dormitorios: 4,
        garagem: true,
        area: 180,
        preco: "R$ 680.000",
        endereco: "Av. Principal, 456 - Jardim das Acácias",
        imagem: "assets/placeholder02.jpg",
        descricao: "Casa ampla com quintal grande, ideal para famílias"
    },
    {
        id: 3,
        titulo: "Apartamento Compacto",
        tipo: "apartamento",
        dormitorios: 2,
        garagem: false,
        area: 55,
        preco: "R$ 280.000",
        endereco: "Rua São José, 789 - Vila Nova",
        imagem: "assets/placeholder03.jpg",
        descricao: "Ótima localização, próximo a comércio e transporte público"
    },
    {
        id: 4,
        titulo: "Casa de Luxo em Condomínio",
        tipo: "casa",
        dormitorios: 5,
        garagem: true,
        area: 250,
        preco: "R$ 1.200.000",
        endereco: "Condomínio Vista Verde, 100 - Alphaville",
        imagem: "assets/placeholder04.jpg",
        descricao: "Casa de alto padrão com piscina, sauna e área gourmet"
    }
];

function obterParametrosURL() {
    const params = new URLSearchParams(window.location.search);
    return {
        tipo: params.get('tipo') || '',
        garagem: params.get('garagem') || '',
        dormitorios: params.get('dormitorios') || '',
        espaco: params.get('espaco') || ''
    };
}

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

function filtrarImoveis(filtros) {
    let imoveisFiltrados = [...imoveisPlaceholder];

    if (filtros.tipo) {
        imoveisFiltrados = imoveisFiltrados.filter(imovel => imovel.tipo === filtros.tipo);
    }

    if (filtros.garagem) {
        const temGaragem = filtros.garagem === 'true';
        imoveisFiltrados = imoveisFiltrados.filter(imovel => imovel.garagem === temGaragem);
    }

    if (filtros.dormitorios) {
        const numDormitorios = parseInt(filtros.dormitorios);
        imoveisFiltrados = imoveisFiltrados.filter(imovel => {
            if (numDormitorios === 5) {
                return imovel.dormitorios >= 5;
            }
            return imovel.dormitorios === numDormitorios;
        });
    }

    if (filtros.espaco) {
        imoveisFiltrados = imoveisFiltrados.filter(imovel => {
            switch (filtros.espaco) {
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

function atualizarTextoFiltros(filtros) {
    const filtrosTexto = document.getElementById('filtros-texto');
    const filtrosAtivos = [];

    if (filtros.tipo) {
        filtrosAtivos.push(filtros.tipo === 'casa' ? 'Casa' : 'Apartamento');
    }
    if (filtros.garagem) {
        filtrosAtivos.push(filtros.garagem === 'true' ? 'Com Garagem' : 'Sem Garagem');
    }
    if (filtros.dormitorios) {
        const num = parseInt(filtros.dormitorios);
        filtrosAtivos.push(`${num >= 5 ? '5+' : num} dormitório${num > 1 ? 's' : ''}`);
    }
    if (filtros.espaco) {
        const espacoTexto = {
            '0-50': '0-50 m²',
            '51-100': '51-100 m²',
            '101-150': '101-150 m²',
            '150+': 'Mais de 150 m²'
        };
        filtrosAtivos.push(espacoTexto[filtros.espaco]);
    }

    if (filtrosAtivos.length > 0) {
        filtrosTexto.textContent = `Filtros aplicados: ${filtrosAtivos.join(' • ')}`;
    } else {
        filtrosTexto.textContent = 'Exibindo todos os imóveis disponíveis';
    }
}

function exibirImoveis(imoveis) {
    const cardsContainer = document.getElementById('cards-container');

    if (imoveis.length === 0) {
        cardsContainer.innerHTML = `
            <div class="col-12">
                <div class="sem-resultados">
                    <i class="bi bi-house-x"></i>
                    <h3>Nenhum imóvel encontrado</h3>
                    <p>Não encontramos imóveis que correspondam aos seus filtros.</p>
                    <a href="index.html" class="btn-nova-busca">
                        <i class="bi bi-arrow-left"></i> Tentar novamente
                    </a>
                </div>
            </div>
        `;
    } else {
        cardsContainer.innerHTML = imoveis.map(criarCardImovel).join('');

        document.querySelectorAll('.btn-ver-mais').forEach((btn, index) => {
            btn.addEventListener('click', () => abrirModal(imoveis[index]));
        });
    }
}

function abrirModal(imovel) {
    document.getElementById('modal-titulo').textContent = imovel.titulo;
    document.getElementById('modal-preco').textContent = imovel.preco;
    document.getElementById('modal-endereco').textContent = imovel.endereco;
    document.getElementById('modal-dormitorios').textContent = imovel.dormitorios;
    document.getElementById('modal-area').textContent = `${imovel.area} m²`;
    document.getElementById('modal-garagem').textContent = imovel.garagem ? 'Sim' : 'Não';
    document.getElementById('modal-tipo').textContent = imovel.tipo === 'casa' ? 'Casa' : 'Apartamento';
    document.getElementById('modal-descricao').textContent = imovel.descricao;

    const badgeTipo = document.getElementById('modal-tipo-badge');
    badgeTipo.textContent = imovel.tipo === 'casa' ? 'Casa' : 'Apartamento';

    const todasImagens = [
        'assets/placeholder01.jpg',
        'assets/placeholder02.jpg',
        'assets/placeholder03.jpg',
        'assets/placeholder04.jpg'
    ];

    const carouselInner = document.getElementById('carousel-inner');
    const carouselIndicators = document.getElementById('carousel-indicators');

    carouselInner.innerHTML = todasImagens.map((img, index) => `
        <div class="carousel-item ${index === 0 ? 'active' : ''}">
            <img src="${img}" alt="${imovel.titulo} - Foto ${index + 1}">
        </div>
    `).join('');

    carouselIndicators.innerHTML = todasImagens.map((_, index) => `
        <button type="button" data-bs-target="#carouselImovel" data-bs-slide-to="${index}" 
                ${index === 0 ? 'class="active" aria-current="true"' : ''} 
                aria-label="Slide ${index + 1}"></button>
    `).join('');

    const modal = new bootstrap.Modal(document.getElementById('modalImovel'));
    modal.show();
}

document.addEventListener('DOMContentLoaded', () => {
    const filtros = obterParametrosURL();
    atualizarTextoFiltros(filtros);

    const imoveisFiltrados = filtrarImoveis(filtros);
    exibirImoveis(imoveisFiltrados);
});
