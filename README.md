# CrystalBot
A bot for Tibia based on mouse and keyboard movement simulation which works even while the game is minimised.


# Introdução

Este foi o primeiro bot que eu criei para tibia, parte do código foi reutilizado de um projeto open source que não funcionava, porém, que me serviu como base para arquitetura do projeto, pois na época eu estava aprendendo programação orientada à objetos e foi útil ter um projeto como referência.

O Crystal Bot utilizava a API do windows para enviar sinais até a janela do jogo que simulavam interações com teclado e mouse.
Uma grande feature que implementei foi tornar essa funcionalidade funcional até mesmo quando o jogo estava minimizado e sem interferir no ponteiro do mouse do usuário. Desta forma era possível continuar utilizando o computador, e até mesmo jogar outros jogos, enquanto o bot estava rodando.

Comecei o projeto por volta de agosto de 2015, usei ele por um tempo como um utilitário pessoal para me auxiliar no servidor que eu jogava na época, para o qual não existiam bots públicos disponíveis. Um tempo depois o bot passou a ter features tão úteis que as pessoas que sabiam que eu estava desenvolvendo esse projeto começaram a perguntar se não podiam pagar pra usa-lo, pois ele realmente era bem útil. Achei a ideia interessante e acabei criando um sistema de licenciamento que não precisava de servidor http próprio ( usei criptografia e o pastebin para fazer o setup disso. ) e acabei realmente vendendo licenças do bot por alguns meses.

Esse aqui era um vídeo promocional que usei para propagar o bot quando tornei ele um produto comercial: https://www.youtube.com/watch?v=Mv4NaqTr8Vk


# Features implementadas no bot:
##### Cavebot:

Com o crystal bot era possível deixar o player caçando criaturas no jogo e coletando recursos de forma automática.

  * Video: https://www.youtube.com/watch?v=XSlrbLCqZF8

##### Aimbot para mirar e acertar runas no alvo de forma automática

Nesse jogo, para acertar runas em outros players ou para usar runas de cura em si mesmo, era necessário clicar com o botão direito na runa e em seguida clicar no alvo. Com esse bot era possível automatizar esse processo.

  * Video: https://www.youtube.com/watch?v=v5Z53JHQOiI
  
##### Scripting 
Funcionava de maneira bem simples, usando os mesmos comandos que eram aceitos na tela de hotkeys.

##### Tagging 
Para nomear pontos específicos da tela e torna-los disponívels aos comandos.

##### Detecção de players na tela
Era comum deixar o player na porta de casa fazendo runas com o bot ou treianndo alguma coisa de forma automática. Com essa funcionalidade era possível programar o bot para deslogar o player ou entrar em casa quando estivesse fazendo runas e alguém aparecece na tela.

##### SpyDown e SpyUp 
Era possível ver o que se passava em outros andares do jogo devido ao client do jogo receber informações sobre os outros andares e mante-los na memória ram. Assim, trocando-se o valor da variável que representava o andar atual do player, o jogo automaticamente renderizava o andar escolhido.

##### Outros:
* Hotkeys
* Light hack
* Eat food
* Caputra de tela e detecção de objetos mesmo com a janela do jogo minimizada.

##### Sobre o estado atual do bot

Provavelmente esse bot ainda funciona nas versões de tibia que possuem os endereços iguais aos que estão registrados no arquivo [Version.cs](https://github.com/PimentelM/CrystalBot/blob/master/ClassicBotter/Addresses/Version.cs) . Por ser um bot que utiliza a simulação de mouse e teclado, muito provavelmente ele funcionaria em qualquer versão do tibia ou até mesmo em outro jogo semelhante como o Zezenia, bastando fazer as modificações necessárias para modelar corretamente as estruturas de dados do jogo e criar a api de interação com esses objetos.

O código de interação com outras janelas usando a API do windows é bastante útil e já utilizei ele para resolver um desafio de CTF chamado iMathze onde era necessário resolver um labirinto e enviar a sequência de teclas para que ele fosse completado em menos de x segundos.

Video: https://www.youtube.com/watch?v=EAIK2Rw8Cv0
