﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Incipiens.Modulos.Endereco.Database.Properties {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Incipiens.Modulos.Endereco.Database.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a CREATE TABLE EstadoFederal (
        ///    IdEstado varchar(2) NOT NULL,
        ///    Uf varchar(2) NOT NULL,
        ///    Nome varchar(60) NOT NULL,
        ///    PRIMARY KEY (IdEstado)
        ///);
        ///CREATE TABLE Municipio (
        ///    IdMunicipio varchar(8) NOT NULL,
        ///    Nome varchar(120) NOT NULL,
        ///    IdEstado varchar(2) NOT NULL,
        ///    PRIMARY KEY (IdMunicipio),
        ///    CONSTRAINT FK_Municipio_EstadoFederal_IdEstado FOREIGN KEY (IdEstado) REFERENCES EstadoFederal (IdEstado) ON DELETE RESTRICT
        ///);
        ///CREATE TABLE Endereco (
        ///    IdEndereco int UNSIGNED NOT [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        public static string EnderecoV1 {
            get {
                return ResourceManager.GetString("EnderecoV1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a ALTER TABLE Endereco
        ///	MODIFY IdMunicipio VARCHAR(8) NOT NULL;.
        /// </summary>
        public static string EnderecoV2 {
            get {
                return ResourceManager.GetString("EnderecoV2", resourceCulture);
            }
        }
    }
}
