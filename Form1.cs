private void UpdateGenerationColor()
{
    colornumber++;
    colornumber = colornumber % ColorPalettes.Spectrum360.Length;
    squareModelAlive.BackColor = Color.FromArgb(
        ColorPalettes.Spectrum360[colornumber].red,
        ColorPalettes.Spectrum360[colornumber].green,
        ColorPalettes.Spectrum360[colornumber].blue);
}
