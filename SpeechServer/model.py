import torch.nn.functional as F
import torch.nn as nn

class ResidualTDNNBlock(nn.Module):
    def __init__(self, channels, kernel_size, dilation=1, dropout=None,bypass_scale = 0.5):
        super().__init__()
        self.tdnn1 = TDNNBlock(channels, channels, kernel_size, dilation, dropout)
        self.tdnn2 = TDNNBlock(channels, channels, kernel_size, dilation, dropout)
        self.bypass_scale = bypass_scale

    def forward(self, x):
        residual = x
        out = self.tdnn1(x)
        out = self.tdnn2(out)
        return out + residual * self.bypass_scale

class TDNNBlock(nn.Module):
    def __init__(self, in_channels, out_channels, kernel_size, dilation=1, dropout=None, activation = True):
        super().__init__()
        padding = dilation * (kernel_size // 2)
        self.conv = nn.Conv1d(in_channels, out_channels, kernel_size,
                            dilation=dilation, padding=padding)
        self.ln = nn.LayerNorm(out_channels)
        self.relu = nn.ReLU() if activation is True else nn.Identity()# wer 17
        self.dropout = nn.Dropout1d(dropout) if dropout is not None else nn.Identity()

    def forward(self, x):  # x: [B, C_in, T]
        out = self.conv(x)           # [B, C_out, T]
        out = out.transpose(1,2)
        out = self.ln(out)
        out = out.transpose(1,2)
        out = self.relu(out)
        out = self.dropout(out)
        return out

class TDNN(nn.Module):
    def __init__(self, input_dim, output_dim):
        super(TDNN, self).__init__()
        self.tdnn1 = TDNNBlock(input_dim, 400, kernel_size=7,dilation=1)
        self.tdnn2 = TDNNBlock(400, 400, kernel_size=5, dilation=2,dropout=0.2)
        self.tdnn3 = TDNNBlock(400, 400, kernel_size=5, dilation=3,dropout=0.3)
        self.res1 = ResidualTDNNBlock(400, kernel_size=3, dilation=2)
        self.res2 = ResidualTDNNBlock(400, kernel_size=3,dilation=1)
        self.res3 = ResidualTDNNBlock(400, kernel_size=3,dilation=1)
        self.tdnn4 = TDNNBlock(400, 300, kernel_size=1,dropout=0.3)
        self.tdnn5 = TDNNBlock(300, 300, kernel_size=1,dropout=0.4)
        self.tdnn6 = TDNNBlock(300, output_dim, kernel_size=1, activation = False)
 
    def forward(self, x):  # x: [B, T, F]
        x = x.transpose(1, 2)  # [B, F, T]
        x = self.tdnn1(x)
        x = self.tdnn2(x)
        x = self.tdnn3(x)
        x = self.res1(x)
        x = self.res2(x)
        x = self.res3(x)
        x = self.tdnn4(x)
        x = self.tdnn5(x)
        x = self.tdnn6(x)
        x = x.transpose(1, 2)  # [B, T, F]
        return F.log_softmax(x, dim=-1)