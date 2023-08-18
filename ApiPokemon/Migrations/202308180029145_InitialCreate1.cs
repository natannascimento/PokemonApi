namespace ApiPokemon.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CapturedPokemonModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PokemonName = c.String(),
                        MasterPokemonName = c.String(),
                        DateCapture = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PokemonMasterModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        CPF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.MestrePokemons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MestrePokemons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Idade = c.Int(nullable: false),
                        CPF = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.PokemonMasterModels");
            DropTable("dbo.CapturedPokemonModels");
        }
    }
}
