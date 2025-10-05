using WebApi.Models;
using MongoDB.Driver;

namespace WebApi.Database.Seeders;

public class MedicalExamSeeder : ISeeder
{
    private readonly IApplicationDbContext _context;

    public MedicalExamSeeder(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        var collection = _context.MedicalExams;

        var exists = await collection.Find(Builders<MedicalExam>.Filter.Empty).AnyAsync();
        if (exists) return;

        var medicalExams = new List<MedicalExam>
        {
            new MedicalExam
            {
                Name = "Hemograma Completo",
                Indication = "Avaliação geral da saúde, diagnóstico de anemias e infecções.",
                PreparationRequired = "Jejum de 8 horas pode ser recomendado.",
                DurationTime = "5 minutos",
                DeliveryDeadline = "1 dia útil",
                Description = "Exame de sangue que avalia os componentes celulares, como glóbulos vermelhos, brancos e plaquetas."
            },
            new MedicalExam
            {
                Name = "Eletrocardiograma (ECG)",
                Indication = "Avaliação de arritmias e doenças cardíacas.",
                PreparationRequired = "Evitar uso de cremes na pele antes do exame.",
                DurationTime = "10 minutos",
                DeliveryDeadline = "Imediato",
                Description = "Avalia a atividade elétrica do coração para detectar arritmias e outras condições cardíacas."
            },
            new MedicalExam
            {
                Name = "Ultrassonografia Abdominal",
                Indication = "Avaliação de órgãos abdominais como fígado, rins e vesícula.",
                PreparationRequired = "Jejum de 6 a 8 horas.",
                DurationTime = "20 minutos",
                DeliveryDeadline = "2 dias úteis",
                Description = "Exame de imagem que permite visualizar órgãos abdominais como fígado, rins e vesícula."
            },
            new MedicalExam
            {
                Name = "Mamografia",
                Indication = "Rastreamento e diagnóstico precoce do câncer de mama.",
                PreparationRequired = "Não usar desodorante ou talco nas axilas.",
                DurationTime = "15 minutos",
                DeliveryDeadline = "3 dias úteis",
                Description = "Exame de imagem das mamas utilizado para detectar precocemente o câncer de mama."
            },
            new MedicalExam
            {
                Name = "Papanicolau",
                Indication = "Detecção de alterações nas células do colo do útero.",
                PreparationRequired = "Evitar relações sexuais e uso de duchas 48h antes.",
                DurationTime = "10 minutos",
                DeliveryDeadline = "7 dias úteis",
                Description = "Exame ginecológico para detectar alterações nas células do colo do útero."
            },
            new MedicalExam
            {
                Name = "Raio-X de Tórax",
                Indication = "Diagnóstico de doenças pulmonares e cardíacas.",
                PreparationRequired = "Remover objetos metálicos do tórax.",
                DurationTime = "5 minutos",
                DeliveryDeadline = "1 dia útil",
                Description = "Imagem dos pulmões e estruturas torácicas para diagnóstico de doenças respiratórias."
            },
            new MedicalExam
            {
                Name = "Teste de Glicemia",
                Indication = "Diagnóstico e controle do diabetes.",
                PreparationRequired = "Jejum de 8 horas.",
                DurationTime = "5 minutos",
                DeliveryDeadline = "Imediato",
                Description = "Mede os níveis de glicose no sangue para diagnóstico e controle do diabetes."
            },
            new MedicalExam
            {
                Name = "Colesterol Total e Frações",
                Indication = "Avaliação do risco cardiovascular.",
                PreparationRequired = "Jejum de 12 horas.",
                DurationTime = "5 minutos",
                DeliveryDeadline = "2 dias úteis",
                Description = "Avalia os níveis de colesterol LDL, HDL e triglicerídeos no sangue."
            },
            new MedicalExam
            {
                Name = "Exame de Urina (EAS)",
                Indication = "Detecção de infecções urinárias e doenças renais.",
                PreparationRequired = "Coletar a primeira urina da manhã.",
                DurationTime = "5 minutos",
                DeliveryDeadline = "1 dia útil",
                Description = "Analisa características físicas e químicas da urina para detectar infecções e outras condições."
            },
            new MedicalExam
            {
                Name = "Tomografia Computadorizada",
                Indication = "Avaliação detalhada de órgãos e estruturas internas.",
                PreparationRequired = "Jejum de 4 horas; pode ser necessário contraste.",
                DurationTime = "30 minutos",
                DeliveryDeadline = "3 dias úteis",
                Description = "Exame de imagem detalhado que permite visualizar estruturas internas do corpo em cortes transversais."
            }
        };

        await collection.InsertManyAsync(medicalExams);
    }
}
