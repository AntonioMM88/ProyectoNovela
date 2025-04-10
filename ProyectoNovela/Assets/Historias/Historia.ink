//Variables Globales, se usan en cualquier sitio
VAR NombreJugador = "Steve"

VAR Ataque = false
VAR Cobarde = false


VAR PuntosVida = 50

//Funciones externas
EXTERNAL ShowCharacter(characterName, position, mood)
EXTERNAL HideCharacter(characterName)
EXTERNAL ChangeMood(characterName, mood)
EXTERNAL ChangeScene(scene)
EXTERNAL SwitchSong()






->nudo_inicio  //empieza la historia

== nudo_inicio ==


Oh no {NombreJugador} 
¿Creeper? Oh damn
{ShowCharacter("Narrador", "Center" , "Happy")}
Deprisa reacciona
{HideCharacter("Narrador")}


-> nudo_2

== nudo_2 ==

->Eleccion


=== Eleccion ===
{ShowCharacter("Narrador", "Center" , "Happy")}
Atacar o huir
***Atacar
->Atacar

***Huir
->Huir

=== Atacar ===
#speaker: Creeper
Tssssk
~Ataque = true//actualizo la variable
->Continuacio_De_Conversacion_Bien
=== Huir ===
#speaker: Creeper
{SwitchSong()}
~Ataque = false//actualizo la variable
~PuntosVida -= 20
Es muy rapido {ChangeMood("Narrador", "Angry")}
->Continuacio_De_Conversacion_Mal

=== Continuacio_De_Conversacion_Bien ===
***Espadazo
->Continuacio_De_Conversacion_Bien2
***Hazhazo
->Continuacio_De_Conversacion_Bien2

=== Continuacio_De_Conversacion_Mal ===
***Seguir corriendo
~Cobarde = true
->Continuacio_De_Conversacion_Ma2
***Pegarle
{ChangeMood("Narrador", "Freak")}

->Continuacio_De_Conversacion_Ma2

=== Continuacio_De_Conversacion_Bien2 ===
->Defensa_del_mi_Personaje

=== Continuacio_De_Conversacion_Ma2 ===
{ Cobarde: Sssssss }#speaker: Creeper
->Defensa_del_mi_Personaje

=== Defensa_del_mi_Personaje ===
{PuntosVida < 50: Debes luchar}#speaker: Yo
{ Cobarde: Usa tus armas  }#speaker: Yo
{ Atacar: Rematalo }#speaker: Yo
->Continuar


=== Continuar ===
{ Atacar: Ksssst }#speaker: Creeper
{HideCharacter("Narrador")}
{not Atacar: Ssssssss}#speaker: Creeper
{not Atacar: ->Eleccion}
->END

