using System;

public class PongGame
{
	//variáveis (escopo mais abrangente)
	static int player1Posicao = 0; //eixo horizontal (top)
	static int player2Posicao = 0;
	static int playerPontos = 0;
	static int computadorPontos = 0;

	static int bolaX = 0; //left = coluna
	static int bolaY = 0; //top = linha
	static bool bolaSubindo = true; //flag
	static bool bolaDirecaoDireita = true;

	static int raqueteTamanho = 4;

	const int ALTURA = 20; //Height
	const int LARGURA = 60; //Width

	static void Main(string[] args)
	{

		//inicilizar o jogo
		inicializar();//chamando o método (operação ou função)

		//GameLoop
		while (true)
		{
			//nosso algoritmo para o jogo 
			//vamos utilizar o método dividir para conquistar

			//limpar a tela
			Console.Clear();

			//verificar o que foi digitado e fazer o movimento (tecla para cima, para baixo)
			if (Console.KeyAvailable) //digitou alguma coisa..
			{
				ConsoleKeyInfo keyInfo = Console.ReadKey();
				//movimento do jogador
				moverJogador(keyInfo);
			}

			//computador vai fazer seu movimento (IA do jogo)
			movimentoComputador();

			//movimentação da bolinha
			movimentoBola();

			//desenha a tela
			desenhaTela();

			//colocar em espera (relógio)
			System.Threading.Thread.Sleep(90);
		}

	} 

	static void inicializar()
	{
		//ajustando o tamanho da janela
		Console.WindowHeight = ALTURA;
		Console.WindowWidth = LARGURA;

		//posicionar as raquetes na tela (jogadores)
		player1Posicao = ((Console.WindowHeight / 2) - (raqueteTamanho / 2));
		player2Posicao = ((Console.WindowHeight / 2) - (raqueteTamanho / 2));

		//posicionar a bola
		inicializaBola();
	}

	static void moverJogador(ConsoleKeyInfo keyInfo)
	{
		//fazer o movimento (tecla para cima, para baixo)
		if (keyInfo.Key == ConsoleKey.UpArrow)
		{
			//vai para cima
			if (player1Posicao > 0) //não chegou ao topo
			{
				player1Posicao--;
			}
		}
		else if (keyInfo.Key == ConsoleKey.DownArrow)
		{
			//vai para baixo
			if (player1Posicao < (Console.WindowHeight - raqueteTamanho))
			{
				player1Posicao++;
			}
		}
	}

	static void inicializaBola()
	{
		bolaX = Console.WindowWidth / 2;
		bolaY = Console.WindowHeight / 2;
	}

	static void movimentoComputador()
	{
		//verificar a direção da bola
		if (bolaSubindo == true)
		{
			//se a bola estiver subindo, o computador deverá subir a raquete
			if (player2Posicao > 0)
			{
				player2Posicao--;
			}
		}
		else
		{
			//descendo...
			//senão irá baixar a raquete
			if (player2Posicao < (Console.WindowHeight - raqueteTamanho))
			{
				player2Posicao++;
			}
		}
	}

	static void movimentoBola()
	{
		//controlar se a bola chegou nos extremos (topo ou base da janela)
		//verificar se chegou no topo
		if (bolaY == 0)
		{
			bolaSubindo = false;
		}
		if (bolaY == Console.WindowHeight - 1)
		{
			bolaSubindo = true;
		}

		//verificar se colidiu com as com as paredes (direito ou esquerdo)
		//verificar se colidiu com o lado direito (jogador marca ponto)
		if (bolaX == Console.WindowWidth - 1) //direita
		{
			Console.Beep(1000, 100);
			//marca o ponto
			playerPontos++;
			//mudar a direção da bola (esquerda ou direita)
			bolaDirecaoDireita = false;//esquerda
									   //mudar direção da bola (subir ou descer)
			bolaSubindo = true;
			//notificar o ponto na tela
			imprimePonto("Ponto para o jogador.");
			//inicializar a bola
			inicializaBola();
			//começar novamente...
			Console.ReadKey();
		}
		//computar marca (colidiu na parede esquerda)
		if (bolaX == 0) //chegou na parede esquerda
		{
			Console.Beep(1000, 100);
			computadorPontos++;
			bolaDirecaoDireita = true;
			bolaSubindo = true;
			imprimePonto("Ponto para o Computador.");
			inicializaBola();
			Console.ReadKey();
		}

		//verificar se colidiu com a raquete (jogador como computador = jogador2)
		//verifica colisão com raquete do jogador (lado esquerdo)
		if (bolaX < 3) //mesma coluna da raquete
		{
			if ((bolaY >= player1Posicao) && (bolaY < (player1Posicao + raqueteTamanho)))
			{
				bolaDirecaoDireita = true;
				Console.Beep(120, 700);
			}
		}
		//verifica colisão com raquete do computador (lado direito)
		if (bolaX >= Console.WindowWidth - raqueteTamanho)
		{
			if ((bolaY >= player2Posicao) && (bolaY < (player2Posicao + raqueteTamanho)))
			{
				bolaDirecaoDireita = false;
				Console.Beep(120, 700);
			}
		}
		//manter o movimento da bola de subir e descer
		if (bolaSubindo)
		{
			bolaY--;
		}
		else
		{
			bolaY++;
		}

		//manter o movimento da bola (direita e esquerda)
		if (bolaDirecaoDireita)
		{
			bolaX++;
		}
		else
		{
			bolaX--;
		}

	}

	static void imprimePonto(string mensagem)
	{
		Console.SetCursorPosition(Console.WindowWidth / 4, Console.WindowHeight / 2);
		Console.WriteLine(mensagem);
	}

	static void desenhaTela()
	{
		//desenhar todos os elementos (Game Objects)

		//desenhar o player1
		for (int i = player1Posicao; i < (player1Posicao + raqueteTamanho); i++)
		{
			desenha(0, i, '|');
			desenha(1, i, '|');
		}

		//desenhar o player2
		for (int i = player2Posicao; i < (player2Posicao + raqueteTamanho); i++)
		{
			desenha(Console.WindowWidth - 1, i, '|');
			desenha(Console.WindowWidth - 2, i, '|');
		}

		//desenhar o bola \u25A0 (unicode)
		desenha(bolaX, bolaY, 'O');

		//desenhar o placar
		Console.SetCursorPosition(((Console.WindowWidth - 1) / 2) - 2, 0);
		Console.WriteLine("{0} - {1}", playerPontos, computadorPontos);

		//estético... para o cursor ficar no quanto inferior da tela
		Console.SetCursorPosition((Console.WindowWidth - 1), (Console.WindowHeight - 1));

	}

	static void desenha(int x, int y, char caracter)
	{
		Console.SetCursorPosition(x, y);
		Console.Write(caracter);
	}


}
