using System.Drawing;
using System.Text;
using System;

namespace DataModel.Models;

public class BodyMotionAnalysis動作分析Model
{
    public string TorsoRotation軀幹旋轉 { get; set; }
    public string TrunkStability軀幹穩定 { get; set; }
    public string ForwardBendOfTrunk軀幹前彎 { get; set; }
    public string TrunkStretch軀幹伸展 { get; set; }
    public string SideCurvatureOfTrunk軀幹側彎 { get; set; }
    public MotionAnalysisRadarChartModel MuscleStrength肌力強度RadarChartModel { get; set; } = new();
    public MotionAnalysisRadarChartModel CMuscularEndurance肌耐力RadarChartModel { get; set; } = new();
    public MotionAnalysisRadarChartModel OverallMusclePerformance肌肉綜合表現RadarChartModel { get; set; } = new();
}





